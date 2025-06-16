using Blog_API.Models;

namespace Blog_API.Repository_Storage.Repo_Interface
{
    public interface IBlogRatingRepository: IRepository<BlogRating>
    {
        Task Update(BlogRating blogRating);
    }
}
