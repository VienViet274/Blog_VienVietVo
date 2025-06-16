using Blog_Client.Models;
using Blog_Client.Models.DTO;

namespace Blog_Client.Service.IService
{
    public interface ICategoryService
    {
        Task<APIResponse> GetAll(string token, string Search);
        Task<APIResponse> GetOne(int id, string token);
        Task<APIResponse> Create(CategoryCreateDTO category, string token);
        Task<APIResponse> Update(CategoryUpdateDTO category, string token);
        Task<APIResponse> Delete(int id, string token);
    }
}
