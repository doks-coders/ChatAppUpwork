using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models.Response;

namespace ChatUpdater.ApplicationCore.Services.Interfaces
{
    public interface IDemoService
    {
        Task<ApiResponseModal<DemoResponse>> GetDemoResponse();

        Task<ApiResponseModal<List<DemoResponse>>> GetDemoList();

        Task<ApiResponseModal> UserNotFound();

        Task<ApiResponseModal> IncorrectCredentials();
    }
}
