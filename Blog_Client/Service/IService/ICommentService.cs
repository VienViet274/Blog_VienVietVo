using Blog_Client.Models;
using Blog_Client.Models.DTO;

namespace Blog_Client.Service.IService
{
    public interface ICommentService
    {
        Task<APIResponse> AddComment(CommentDTO commentDTO, string token);
        Task<APIResponse> GetAllComments(int blogID, string token);
        Task<APIResponse> GetOneComment(int blogID, string token);
        Task<APIResponse> ApproveComment(int commentId, string token);
    }
}
