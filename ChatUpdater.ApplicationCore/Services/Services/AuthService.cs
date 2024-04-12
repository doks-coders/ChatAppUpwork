using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Infrastructure.Validators.Account;
using ChatUpdater.Models;
using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Serilog;



namespace ChatUpdater.ApplicationCore.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MessageMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _userService;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IUsersService usersService, ITokenService tokenService, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
            _userService = usersService;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }





        public async Task<ApiResponseModal<bool>> Register(RegisterUserRequest registerUser)
        {
            if (await _userManager.Users.CountAsync() > 10) throw new ApiErrorException(BaseErrorCodes.MaximumUsersLimit);
            var registerUserValidator = new RegisterUserValidator();
            var validation = await registerUserValidator.ValidateAsync(registerUser);
            if (!validation.IsValid) throw new ApiErrorException(validation.Errors);

            var user = new ApplicationUser { UserName = registerUser.UserName, Email = registerUser.Email, PhoneNumber = registerUser.PhoneNumber };

            if (await _userManager.FindByEmailAsync(registerUser.Email) != null)
            {
                throw new ApiErrorException(BaseErrorCodes.EmailTaken);
            }



            var res = await _userManager.CreateAsync(user);
            user.EmailConfirmed = false;


            if (res.Succeeded)
            {
                await SendSetPasswordEmail(user);

                return await ApiResponseModal<bool>.SuccessAsync(true);
            }

            if (res.Errors.FirstOrDefault(i => i.Code == "DuplicateUserName") != null)
            {
                throw new ApiErrorException(BaseErrorCodes.UserNameExists);
            }

            throw new ApiErrorException(BaseErrorCodes.IdentityError);
        }

        public async Task<ApiResponseModal<AuthUserResponse>> Login(LoginUserRequest loginUser)
        {
            var loginUserValidator = new LoginUserValidator();
            var validation = await loginUserValidator.ValidateAsync(loginUser);
            if (!validation.IsValid) throw new ApiErrorException(validation.Errors);

            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user == null) throw new ApiErrorException(BaseErrorCodes.UserNotFound);

            var matches = await _userManager.CheckPasswordAsync(user, loginUser.Password);

            if (matches)
            {
                return await ApiResponseModal<AuthUserResponse>.SuccessAsync(new AuthUserResponse(
                    Token: await _tokenService.CreateToken(user)
                    ));
            }
            throw new ApiErrorException(BaseErrorCodes.InvalidPassword);
        }


        private string GetUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";

        }

        private async Task SendSetPasswordEmail(ApplicationUser applicationUser)
        {
            if (string.IsNullOrEmpty(applicationUser.Email))
                throw new ApiErrorException(BaseErrorCodes.EmailNull);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var verifyUrl = QueryHelpers.AddQueryString(
                $"{GetUrl()}/verify-email", "token", token);

            verifyUrl = QueryHelpers.AddQueryString(
                verifyUrl, "userid", applicationUser.Id.ToString());


            Log.Information(verifyUrl);

            string subject = "Set Password";
            string htmlMessage = @$"
   Thanks for registering with us. 
   To set a password for your account please click ""{verifyUrl}"".
   Regards,


";
            try
            {
                await _emailSender.SendEmailAsync(applicationUser.Email, htmlMessage, subject);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

        }



        public async Task<bool> ConfirmEmail(string userId, string token)
        {

            var decodedUserId = Guid.Parse(userId);

            var user = await _unitOfWork.Users.Get(u => u.Id == decodedUserId);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _unitOfWork.Save();
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<ApiResponseModal<AuthUserResponse>> SetUserPassword(SetPasswordRequest passwordRequest)
        {
            var setPasswordRequestValidator = new SetPasswordRequestValidator();
            var validation = await setPasswordRequestValidator.ValidateAsync(passwordRequest);
            if (!validation.IsValid) throw new ApiErrorException(validation.Errors);

            var user = await _unitOfWork.Users.Get(u => u.Id == passwordRequest.UserId);

            if (user == null) throw new ApiErrorException(BaseErrorCodes.UserNotFound);

            if (passwordRequest.Password != passwordRequest.ConfirmPassword) throw new ApiErrorException(BaseErrorCodes.InvalidPassword);

            if (user.EmailConfirmed == true && user.PasswordLock == false)
            {
                var res = await _userManager.AddPasswordAsync(user, passwordRequest.Password);
                if (!res.Succeeded) throw new ApiErrorException(BaseErrorCodes.InvalidPassword);

                user.PasswordLock = true;
                await _unitOfWork.Save();


                return await ApiResponseModal<AuthUserResponse>.SuccessAsync(new AuthUserResponse(
                    Token: await _tokenService.CreateToken(user)
                    ));
            }

            throw new ApiErrorException(BaseErrorCodes.IdentityError);
        }
    }
}
