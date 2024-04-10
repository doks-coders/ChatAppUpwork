using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;

namespace ChatUpdater.Controllers
{
    public class AuthController : ParentController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;
        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IAuthService authService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ApiResponseModal<bool>> Register([FromBody] RegisterUserRequest registerUser)
        => await _authService.Register(registerUser);


        [HttpPost("login")]
        public async Task<ApiResponseModal<AuthUserResponse>> LoginUser([FromBody] LoginUserRequest loginUser)
        => await _authService.Login(loginUser);


        [HttpPost("set-password")]
        public async Task<ApiResponseModal<AuthUserResponse>> SetPassword([FromBody] SetPasswordRequest setPassword)
        => await _authService.SetUserPassword(setPassword);

    }
}
