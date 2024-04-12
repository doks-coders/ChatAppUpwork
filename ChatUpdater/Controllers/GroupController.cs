using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Extensions;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatUpdater.Controllers
{
    public class GroupController : ParentController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MessageMapper _mapper;
        private readonly IGroupService _groupService;
        public GroupController(IUnitOfWork unitOfWork, IGroupService groupService)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
            _groupService = groupService;
        }

        [HttpPost("create-group")]
        public async Task<ApiResponseModal<bool>> CreateGroupChat([FromBody] CreateGroupChatRequest createGroupChat)
        => await _groupService.CreateGroup(createGroupChat, User.GetUserId());


        [HttpGet("get-groups")]
        public async Task<ApiResponseModal<List<GroupChatResponse>>> GetAllGroups()
        => await _groupService.GetAllGroups();


        [HttpGet("get-my-groups")]
        public async Task<ApiResponseModal<List<GroupChatResponse>>> GetMyGroups()
        => await _groupService.GetMyGroups(User.GetUserId());


        [HttpGet("join-group/{groupId}")]
        public async Task<ApiResponseModal<bool>> JoinGroup(Guid groupId)
        => await _groupService.JoinGroup(User.GetUserId(), groupId);


        [HttpGet("get-joined-groups")]
        public async Task<ApiResponseModal<List<GroupChatResponse>>> GetJoinedGroups()
        => await _groupService.GetJoinedGroups(User.GetUserId());

        [HttpGet("get-group-members/{groupId}")]
        public async Task<ApiResponseModal<List<UserResponse>>> GetGroupMembers(Guid groupId)
        => await _groupService.GetGroupMembers(groupId);



        [HttpGet("search-groups")]
        public async Task<ApiResponseModal<List<GroupChatResponse>>> SearchGroup([FromQuery] string search)
        => await _groupService.SearchGroup(search);








    }
}
