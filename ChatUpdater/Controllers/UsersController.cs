using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Extensions;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatUpdater.Controllers
{
    [Authorize]
    public class UsersController : ParentController
    {

        private readonly IUsersService _userService;

        public UsersController(IUsersService usersService)
        {
            _userService = usersService;
        }
        [HttpGet("get-users")]
        public async Task<ApiResponseModal<List<UserResponse>>> AllUsers()
           => await _userService.GetAllUsers(User.GetUserId());


        [HttpGet("get-id")]
        public async Task<ApiResponseModal<Guid>> GetUserId()
        => await ApiResponseModal<Guid>.SuccessAsync(User.GetUserId());


        [HttpPost("upload-image")]
        public async Task<ApiResponseModal<UploadImageResponse>> UploadImage(IFormFile file, string formerFile)
        => await _userService.UpsertProfileImage(file, formerFile, User.GetUserId());

        [HttpGet("get-user-information")]
        public async Task<ApiResponseModal<UserResponse>> GetUserInformation()
        => await _userService.GetUserInformation(User.GetUserId());

        [HttpGet("search-users")]
        public async Task<ApiResponseModal<List<UserResponse>>> SearchUsers([FromQuery] string search)
        => await _userService.SearchUsers(search);


        [HttpPost("update-user-information")]
        public async Task<ApiResponseModal<bool>> UpdateUserInformation([FromBody] UpdateUserInformationRequest updateUserInformation)
            => await _userService.UpdateUserInformation(updateUserInformation, User.GetUserId());



        //get-user-information
    }
}
