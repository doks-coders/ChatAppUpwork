using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;

namespace ChatUpdater.ApplicationCore.Services.Interfaces
{
    public interface IGroupService
    {


        /// <summary>
        /// This is used for creating groups chats
        /// </summary>
        /// <param name="createGroupChat"></param>
        /// <returns></returns>
        Task<ApiResponseModal<bool>> CreateGroup(CreateGroupChatRequest createGroupChat, Guid adminId);



        Task<ApiResponseModal<List<GroupChatResponse>>> GetAllGroups();

        Task<ApiResponseModal<List<GroupChatResponse>>> GetMyGroups(Guid id);

        Task<ApiResponseModal<bool>> JoinGroup(Guid userId, Guid groupId);

        Task<ApiResponseModal<List<UserResponse>>> GetGroupMembers(Guid groupId);

        Task<ApiResponseModal<List<GroupChatResponse>>> SearchGroup(string predicate);

        Task<ApiResponseModal<List<GroupChatResponse>>> GetJoinedGroups(Guid userId);
    }

}
