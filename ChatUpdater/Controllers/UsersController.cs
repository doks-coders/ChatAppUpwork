using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Extensions;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Response;

namespace ChatUpdater.Controllers
{
    [Authorize]
    public class UsersController : ParentController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MessageMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _userService;

        public UsersController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IUsersService usersService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
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



        //get-user-information
    }
}
