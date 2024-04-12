using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Infrastructure.Validators.User;
using ChatUpdater.Models;
using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatUpdater.ApplicationCore.Services.Services
{
    public class UsersService : IUsersService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MessageMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponseModal<List<UserResponse>>> GetAllUsers(Guid userId)
        {
            var users = await _userManager.Users.Where(e => e.Email != null && e.Id != userId).Select(e =>
            new UserResponse(e.Email,
            e.Id,
            e.ProfilePicture,
            e.RelativeProfilePicture,
            e.UserName,
            e.PhoneNumber
            )).ToListAsync();

            return await ApiResponseModal<List<UserResponse>>.SuccessAsync(users);
        }

        /// <summary>
        /// Saves image to static folder in wwwroot
        /// </summary>
        /// <param name="file"></param>
        /// <param name="formerFile"></param>
        /// <returns></returns>
        public async Task<string> UploadImage(IFormFile file, string formerFile)
        {
            string wwwrootPath = _webHostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = @$"assets";
            string folderPathUrl = @$"{wwwrootPath}/{filePath}";

            if (!string.IsNullOrEmpty(formerFile))
            {
                if (File.Exists(formerFile))
                {
                    File.Delete(formerFile);
                }
            }

            if (!Directory.Exists(folderPathUrl))
            {
                Directory.CreateDirectory(folderPathUrl);
            }

            string photoUrl = $"{folderPathUrl}/{fileName}";

            using (var fileStream = new FileStream(photoUrl, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return photoUrl;
        }


        /// <summary>
        /// Uploads and updates the image for user profiles
        /// </summary>
        /// <param name="file"></param>
        /// <param name="formerFile"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ApiErrorException"></exception>
        public async Task<ApiResponseModal<UploadImageResponse>> UpsertProfileImage(IFormFile file, string formerFile, Guid userId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ApiErrorException(BaseErrorCodes.FileNotFound);
            }

            var photoUrl = await UploadImage(file, formerFile);

            var user = await _unitOfWork.Users.Get(u => u.Id == userId);

            user.ProfilePicture = photoUrl;
            user.RelativeProfilePicture = GetRelativePhotoAssetsPath(photoUrl);

            var relativeUrl = GetRelativePhotoAssetsPath(photoUrl);

            if (await _unitOfWork.Save())
            {
                return await ApiResponseModal<UploadImageResponse>.SuccessAsync(new UploadImageResponse(relativeUrl, photoUrl));
            }
            throw new ApiErrorException(BaseErrorCodes.DatabaseUnknownError);
        }

        public async Task<ApiResponseModal<List<UserResponse>>> SearchUsers(string predicate)
        {
            var foundUsers = await _userManager.Users.Where(u => u.UserName.ToLower().Contains(predicate.ToLower()) || u.Email.ToLower().Contains(predicate.ToLower())).ToListAsync();

            var userResponse = _mapper.ApplicationUserToUserResponse(foundUsers);

            return await ApiResponseModal<List<UserResponse>>.SuccessAsync(userResponse);
        }

        public async Task<ApiResponseModal<UserResponse>> GetUserInformation(Guid userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var userResponse = _mapper.ApplicationUserToUserResponse(user);

            return await ApiResponseModal<UserResponse>.SuccessAsync(userResponse);
        }



        public async Task<ApiResponseModal<bool>> UpdateUserInformation(UpdateUserInformationRequest updateRequest, Guid userId)
        {
            var updateUserRequestValidator = new UpdateUserRequestValidation();
            var validation = await updateUserRequestValidator.ValidateAsync(updateRequest);

            if (!validation.IsValid) throw new ApiErrorException(validation.Errors);

            var user = await _unitOfWork.Users.Get(u => u.Id == userId) ??
                throw new ApiErrorException(BaseErrorCodes.RecordNotFound);

            if (user.UserName != updateRequest.UserName)
            {
                //If incoming username is different, check if the username already exists somewhere else
                var checkUserName = await _unitOfWork.Users.Get(u => u.UserName == updateRequest.UserName);
                if (checkUserName != null)
                {
                    throw new ApiErrorException(BaseErrorCodes.UserNameExists);
                }
            }
            user.UserName = updateRequest.UserName;
            user.PhoneNumber = updateRequest.PhoneNumber;
            if (await _unitOfWork.Save())
            {
                return await ApiResponseModal<bool>.SuccessAsync(true);
            }
            throw new ApiErrorException(BaseErrorCodes.DatabaseUnknownError);
        }

        /// <summary>
        /// In Angular, images are in the assets folder, so we are simply going to make a relative path there
        /// </summary>
        /// <param name="photoUrl"></param>
        /// <returns></returns>
        private string GetRelativePhotoAssetsPath(string photoUrl)
        {
            var photoArray = photoUrl.Split("/assets/");
            return $"assets/{photoArray[1]}";
        }


    }
}
