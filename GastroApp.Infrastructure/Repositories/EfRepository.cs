
using GastroApp.Common;
using GastroApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GastroApp.Infrastructure.Repositories
{
    //Ogolna implementacja IAsyncRepository wykorzystujaca EF Core
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly GastroApp.Infrastructure.GastroAppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public EfRepository(GastroApp.Infrastructure.GastroAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet     = _dbContext.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id)           => await _dbSet.FindAsync(id);
        public async Task<IReadOnlyList<T>> ListAllAsync()    => await _dbSet.ToListAsync();

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}