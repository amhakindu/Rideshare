using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;

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
            var response = new PaginatedResponse<T>(){

                Paginated = await _dbContext.Set<T>().AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
                Count = await _dbContext.Set<T>().CountAsync()
                

            };
            return response;
            
        }

        public async Task<int> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
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
    }