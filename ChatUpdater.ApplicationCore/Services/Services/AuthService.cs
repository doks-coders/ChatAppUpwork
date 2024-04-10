using Microsoft.AspNetCore.Identity;
using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatUpdater.Models;

namespace ChatUpdater.ApplicationCore.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MessageMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IUsersService usersService, ITokenService tokenService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
            _userService = usersService;
            _tokenService = tokenService;
        }

    
        
      

        public async Task<ApiResponseModal<AuthUserResponse>> Register(RegisterUserRequest registerUser)
        {
            var user = new ApplicationUser { UserName = registerUser.UserName, Email = registerUser.Email, PhoneNumber = registerUser.PhoneNumber };
            if (await _userManager.FindByEmailAsync(registerUser.Email) != null) throw new ApiErrorException(BaseErrorCodes.EmailTaken);

            var res = await _userManager.CreateAsync(user, registerUser.Password);
            if (res.Succeeded)
            {
                return await ApiResponseModal<AuthUserResponse>.SuccessAsync( new AuthUserResponse(
                    Email: user.Email,
                    Token: await _tokenService.CreateToken(user)
                    ));
            }
            throw new ApiErrorException(BaseErrorCodes.ValidationError);
        }

        public async Task<ApiResponseModal<AuthUserResponse>> Login(LoginUserRequest loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user == null) throw new ApiErrorException(BaseErrorCodes.UserNotFound);
            var matches = await _userManager.CheckPasswordAsync(user, loginUser.Password);

            if (matches)
            {
                return await ApiResponseModal<AuthUserResponse>.SuccessAsync(new AuthUserResponse(
                    Email: user.Email,
                    Token: await _tokenService.CreateToken(user)
                    ));
            }
            throw new ApiErrorException(BaseErrorCodes.InvalidPassword);
        }

        
    }
}
