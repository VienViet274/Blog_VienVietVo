using Blog_Client.Models;
using Blog_Client.Models.DTO;

namespace Blog_Client.Service.IService
{
    public interface IBlogService
    {
        Task<APIResponse> GetAll(string token, string Search);
        Task<APIResponse> GetOne(int id, string token);

        Task<APIResponse> Update(BlogUpdateDTO blog, string token);
        Task<APIResponse> Delete(int id, string token);
        Task<APIResponse> Create(BlogCreateDTO blogCreateDTO, string token);

    }
}
