using Blog_API.Models;

namespace Blog_API.Repository_Storage.Repo_Interface
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Task Update(Comment comment);
    }
}
