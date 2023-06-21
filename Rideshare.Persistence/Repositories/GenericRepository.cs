using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Persistence.Repositories;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
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

        public async Task<IReadOnlyList<T>> GetAll(int pageNumber, int pageSize)
        {
            return await _dbContext.Set<T>().AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }
    }