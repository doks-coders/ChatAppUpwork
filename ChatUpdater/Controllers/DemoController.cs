using Microsoft.AspNetCore.Mvc;
using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Models.Response;

namespace ChatUpdater.Controllers
{
    public class DemoController : ParentController
    {
        private readonly IDemoService _demoService;
        public DemoController(IDemoService demoService)
        {
            _demoService = demoService;
        }

        [HttpGet("get-info")]
        public async Task<ApiResponseModal<DemoResponse>> GetInfo()
            =>  await _demoService.GetDemoResponse();

        [HttpGet("get-info-list")]
        public async Task<ApiResponseModal<List<DemoResponse>>> GetList()
            => await _demoService.GetDemoList();

        [HttpGet("user-not-found")]
        public async Task<ApiResponseModal> GetUserNotFound()
            => await _demoService.UserNotFound();

        [HttpGet("incorrect-credentials")]
        public async Task<ApiResponseModal> IncorrectCredentials()
            => await _demoService.IncorrectCredentials();
    }
}
