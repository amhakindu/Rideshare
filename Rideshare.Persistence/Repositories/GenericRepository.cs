using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Persistence.Repositories;

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly RideshareDbContext _dbContext;

    public GenericRepository(RideshareDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<int> Add(T entity)
    {
        await _dbContext.AddAsync(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await Get(id);
        return entity != null;
    }

    public async Task<T?> Get(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<PaginatedResponse<T>> GetAll(int pageNumber=1, int pageSize=10)
    {
        var query = _dbContext.Set<T>().AsNoTracking();
        var temp = await query.CountAsync();
        var response = new PaginatedResponse<T>(){
            Value = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            Count = temp
        };
        return response;
        
    }

    public async Task<int> Update(T entity)
    {
        _dbContext.Update(entity);
        return await _dbContext.SaveChangesAsync();
    }
    public async Task<int> Count(){
        return await _dbContext.Set<T>().CountAsync();
    }

    public async Task<double> GetLastWeekPercentageChange()
    {            
        var totalCount = await _dbContext.Set<T>().CountAsync();
        var beforeLastWeekCount = await _dbContext.Set<T>()
            .CountAsync(entity => entity.DateCreated.Day <= DateTime.Today.AddDays(-7).Day);
        
        if(beforeLastWeekCount == 0)
            return 0;
        return (totalCount - beforeLastWeekCount) * 100.0 / beforeLastWeekCount;
    }

    public async Task<Dictionary<int, int>> GetEntityStatistics(int? year, int? month){
		DbSet<T> entities = _dbContext.Set<T>();
		if(month != null && year != null){
			// Weekly
			var temp = await entities.Where(entity => entity.DateCreated.Year == year)
				.Where(entity => entity.DateCreated.Month == month)
				.GroupBy(entity => entity.DateCreated.Day / 7 + 1)
				.ToDictionaryAsync(group => group.Key, group => group.Count());
			for (int i = 1; i <= 5; i++)
			{
				if(!temp.ContainsKey(i))
					temp.Add(i, 0);
			}
			return temp;
		}else if(month == null && year == null){
			// Yearly
			var temp = await entities
				.GroupBy(entity => entity.DateCreated.Year)
				.ToDictionaryAsync(g => g.Key, g => g.Count());
			for (int i = 2023; i <= DateTime.UtcNow.Year; i++)
			{
				if(!temp.ContainsKey(i))
					temp.Add(i, 0);
			}
			return temp;
		}else{   
			// Monthly
			Dictionary<int, int> temp = await entities
				.Where(entity => entity.DateCreated.Year == year)
				.GroupBy(entity => entity.DateCreated.Month)
				.ToDictionaryAsync(g => g.Key, g => g.Count());
			for (int i = 1; i < 13; i++)
			{
				if(!temp.ContainsKey(i))
					temp.Add(i, 0);
			}
			return temp;
		}
	}
}