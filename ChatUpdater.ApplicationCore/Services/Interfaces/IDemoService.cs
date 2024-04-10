using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
