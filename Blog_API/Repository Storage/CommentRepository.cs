using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Repository_Storage
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Comment comment)
        {
            _context.Update(comment);
            await Save();
        }
    }
}
