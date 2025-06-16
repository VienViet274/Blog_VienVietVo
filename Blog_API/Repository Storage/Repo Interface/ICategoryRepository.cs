using Blog_API.Models;

namespace Blog_API.Repository_Storage.Repo_Interface
{
    public interface ICategoryRepository: IRepository<Category>
    {
        public Task Update(Category category);
    }
}
