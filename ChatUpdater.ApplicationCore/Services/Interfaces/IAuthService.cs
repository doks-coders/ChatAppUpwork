using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUpdater.ApplicationCore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponseModal<bool>> Register(RegisterUserRequest registerUser);

        Task<ApiResponseModal<AuthUserResponse>> Login(LoginUserRequest loginUser);

        Task<bool> ConfirmEmail(string userId, string token);

        Task<ApiResponseModal<AuthUserResponse>> SetUserPassword(SetPasswordRequest passwordRequest);
    }
}
