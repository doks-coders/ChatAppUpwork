using Microsoft.AspNetCore.Http;
using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models.Response;

namespace ChatUpdater.ApplicationCore.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ApiResponseModal<List<UserResponse>>> GetAllUsers(Guid userId);
        Task<string> UploadImage(IFormFile file, string formerFile);
        Task<ApiResponseModal<List<UserResponse>>> SearchUsers(string predicate);
        Task<ApiResponseModal<UploadImageResponse>> UpsertProfileImage(IFormFile file, string formerFile, Guid userId);

        Task<ApiResponseModal<UserResponse>> GetUserInformation(Guid userId);
    }
}
