using ChatUpdater.ApplicationCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatUpdater.Controllers
{
    [Route("verify-email")]
    public class VerifyEmailController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VerifyEmailController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userid, string token)
        {
            if (await _authService.ConfirmEmail(userid, token))
            {
                return Redirect($"{GetUrl()}/set-password/{userid}");
            }
            return Json(new
            {
                message = "User could not be confirmed",
                success = false
            });
        }

        private string GetUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";

        }
    }
}
