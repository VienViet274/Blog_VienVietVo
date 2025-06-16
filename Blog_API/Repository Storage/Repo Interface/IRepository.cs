using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Blog_API.Repository_Storage.Repo_Interface
{
    public interface IRepository<T> where T : class
    {
        public Task<List<T>> GetALL(Expression<Func<T,bool>>? expression = null, string? includeProperties = null);
        public Task<T> GetOne(Expression<Func<T, bool>>? expression = null, string? includeProperties = null, bool AsNoTracking = true);
        public Task Create(T entity); 
        public Task Delete(T entity);
        public Task Save();
    }
}
