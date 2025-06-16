using Blog_Client.Models;

namespace Blog_Client.Service.IService
{
    public interface IBaseService
    {
        Task<APIResponse> SendAsync(APIRequest request);
    }
}
