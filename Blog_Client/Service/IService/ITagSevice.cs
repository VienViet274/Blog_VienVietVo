using Blog_Client.Models;
using Blog_Client.Models.DTO;

namespace Blog_Client.Service.IService
{
    public interface ITagSevice
    {
        Task<APIResponse> GetAll(string token, string Search);
        Task<APIResponse> GetOne(int id, string token);
        Task<APIResponse> Create(TagCreateDTO tag, string token);
        Task<APIResponse> Update(TagUpdateDTO tag, string token);
        Task<APIResponse> Delete(int id, string token);
    }
}
