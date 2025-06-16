using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Repository_Storage.Repo_Interface;

namespace Blog_API.Repository_Storage
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly ApplicationDbContext _context;
        public TagRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Tag tag)
        {
            //_context.Update(tag);
            await Save();
        }
    }
}
