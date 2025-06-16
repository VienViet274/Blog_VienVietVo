using Blog_API.Models;

namespace Blog_API.Repository_Storage.Repo_Interface
{
    public interface ITagRepository: IRepository<Tag>
    {
        public Task Update(Tag tag);
    }
}
