using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Infrastructure.Validators.Group;
using ChatUpdater.Models;
using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;

namespace ChatUpdater.ApplicationCore.Services.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MessageMapper _mapper;
        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
        }

        /// <summary>
        /// This is used for creating a group
        /// </summary>
        /// <param name="createGroupChat"></param>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public async Task<ApiResponseModal<bool>> CreateGroup(CreateGroupChatRequest createGroupChat, Guid adminId)
        {
            var createGroupChatValidator = new CreateGroupValidator();
            var validation = await createGroupChatValidator.ValidateAsync(createGroupChat);
            if (!validation.IsValid) throw new ApiErrorException(validation.Errors);

            var group = _mapper.GroupChatRequestToGroupChar(createGroupChat);
            group.AdminId = adminId;
            await _unitOfWork.GroupChats.Add(group);
            if (await _unitOfWork.Save())
            {
                return await ApiResponseModal<bool>.SuccessAsync(true);
            }
            throw new ApiErrorException(BaseErrorCodes.DatabaseUnknownError);


        }

        /// <summary>
        /// Get members of the group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<ApiResponseModal<List<UserResponse>>> GetGroupMembers(Guid groupId)
        {
            var group = await _unitOfWork.GCContacts.GetAll(u => u.AppGroupId == groupId, includeProperties: "AppUser");
            if (group.Count() > 0)
            {
                var users = group.Select(u => u.AppUser).ToList();

                var res = _mapper.ApplicationUserToUserResponse(users);

                return await ApiResponseModal<List<UserResponse>>.SuccessAsync(res);
            }
            return await ApiResponseModal<List<UserResponse>>.SuccessAsync(new List<UserResponse>() { });
        }




        /// <summary>
        /// This is used for retrieving all the groups
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponseModal<List<GroupChatResponse>>> GetAllGroups()
        {
            var groups = await _unitOfWork.GroupChats.GetAll();
            var response = _mapper.GroupChatToGroupChatResponse(groups.ToList());
            return await ApiResponseModal<List<GroupChatResponse>>.SuccessAsync(response);
        }

        /// <summary>
        /// Get your groups that you have created
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponseModal<List<GroupChatResponse>>> GetMyGroups(Guid id)
        {
            var groups = await _unitOfWork.GroupChats.GetAll(u => u.AdminId == id);
            var response = _mapper.GroupChatToGroupChatResponse(groups.ToList());
            return await ApiResponseModal<List<GroupChatResponse>>.SuccessAsync(response);
        }


        /// <summary>
        /// This is used whenever a user joins the group for the first time
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<ApiResponseModal<bool>> JoinGroup(Guid userId, Guid groupId)
        {
            var gCContacts = await _unitOfWork.GCContacts.GetAll(e => e.AppUserId == userId && e.AppGroupId == groupId);
            if (gCContacts.Count() == 0)
            {
                await _unitOfWork.GCContacts.Add(
                new GCContacts() { AppGroupId = groupId, AppUserId = userId });

                if (await _unitOfWork.Save())
                {
                    return await ApiResponseModal<bool>.SuccessAsync(true);
                }
                throw new ApiErrorException(BaseErrorCodes.DatabaseUnknownError);
            }
            return await ApiResponseModal<bool>.SuccessAsync(true);

        }

        /// <summary>
        /// This is used for searching for groups
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<ApiResponseModal<List<GroupChatResponse>>> SearchGroup(string predicate)
        {
            var foundGroups = await _unitOfWork.GroupChats.GetAll(u => u.Name.ToLower().Contains(predicate.ToLower()));

            var groupChatResponses = _mapper.GroupChatToGroupChatResponse(foundGroups.ToList());

            return await ApiResponseModal<List<GroupChatResponse>>.SuccessAsync(groupChatResponses);
        }

        public async Task<ApiResponseModal<List<GroupChatResponse>>> GetJoinedGroups(Guid userId)
        {
            var group = await _unitOfWork.GCContacts.GetAll(u => u.AppUserId == userId, includeProperties: "AppUser,AppGroupChat");
            if (group.Count() > 0)
            {
                var groupChats = group.Select(u => u.AppGroupChat).ToList();

                var res = _mapper.GroupChatToGroupChatResponse(groupChats);
                return await ApiResponseModal<List<GroupChatResponse>>.SuccessAsync(res);
            }

            return await ApiResponseModal<List<GroupChatResponse>>.SuccessAsync(new List<GroupChatResponse>() { });
        }
    }
}
