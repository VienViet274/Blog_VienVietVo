using Blog_API.Data;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blog_API.Repository_Storage
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet =_context.Set<T>();
        }
        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            await Save();
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await Save();
        }

        public async  Task<List<T>> GetALL(Expression<Func<T, bool>>? expression = null, string? includeProperties = null)
        {
            IQueryable<T> list = _dbSet;
            if (expression != null)
            {
                list = list.Where(expression);
            }
            if (includeProperties != null)
            {
                string[] Properties = includeProperties.Split(',');
                foreach (var item in Properties)
                {
                    list = list.Include(item);
                }
            }
            return await list.ToListAsync();
        }

        public async Task<T> GetOne(Expression<Func<T, bool>>? expression = null, string? includeProperties = null, bool AsNoTracking = true)
        {
            IQueryable<T> result =_dbSet;
            if (expression != null)
            {
                 result =  result.Where(expression);
            }
            if (includeProperties != null) 
            {
                string[] Properties = includeProperties.Split(',');
                foreach (var item in Properties)
                {
                    result = result.Include(item);
                }
            }
            if (AsNoTracking)
            {
                return await result.AsNoTracking().FirstOrDefaultAsync();
            }
            else
            {
                return await result.FirstOrDefaultAsync();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

       
    }
}
