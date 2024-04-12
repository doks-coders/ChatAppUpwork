using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using Microsoft.AspNetCore.Http;

namespace ChatUpdater.ApplicationCore.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ApiResponseModal<List<UserResponse>>> GetAllUsers(Guid userId);
        Task<string> UploadImage(IFormFile file, string formerFile);
        Task<ApiResponseModal<List<UserResponse>>> SearchUsers(string predicate);
        Task<ApiResponseModal<UploadImageResponse>> UpsertProfileImage(IFormFile file, string formerFile, Guid userId);

        Task<ApiResponseModal<UserResponse>> GetUserInformation(Guid userId);
        Task<ApiResponseModal<bool>> UpdateUserInformation(UpdateUserInformationRequest updateRequest, Guid userId);
    }
}
