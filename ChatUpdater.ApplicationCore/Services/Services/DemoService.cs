using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Models;
using ChatUpdater.Models.Response;

namespace ChatUpdater.ApplicationCore.Services.Services
{
    public class DemoService : IDemoService
    {
        public async Task<ApiResponseModal<DemoResponse>> GetDemoResponse()
        {
            var response = new DemoResponse
            {
                Text = "Hello"
            };

            return await ApiResponseModal<DemoResponse>.SuccessAsync(response);
        }

        public async Task<ApiResponseModal<List<DemoResponse>>> GetDemoList()
        {
            var response = new List<DemoResponse>() { new DemoResponse() { Text = "New" } };
            return await ApiResponseModal<List<DemoResponse>>.SuccessAsync(response);
        }



        public async Task<ApiResponseModal> UserNotFound()
        {
            throw new ApiErrorException(BaseErrorCodes.UserNotFound);
        }

        public async Task<ApiResponseModal> IncorrectCredentials()
        {

            throw new ApiErrorException(BaseErrorCodes.IncorrectCredentials);
        }


    }
}
