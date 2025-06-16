using Blog_API.Models;

namespace Blog_API.Repository_Storage.Repo_Interface
{
    public interface IBlogRepository: IRepository<Blog>
    {
        Task Update(Blog blog);
        
    }
}
