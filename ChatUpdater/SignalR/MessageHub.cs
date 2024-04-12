using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Extensions;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatUpdater.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly MessageMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public MessageHub(IUnitOfWork unitOfWork, IMessageService messageService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _userManager = userManager;
            _mapper = new MessageMapper();
        }

        /// <summary>
        /// Creating and Maintaining a connection between two users i.e Reciever and User
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public override async Task<Task> OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            string? RecieverId = httpContext?.Request.Query["RecieverId"];
            string? isGroup = httpContext?.Request.Query["isGroup"];

            if (RecieverId == null) throw new HubException("No User");

            string RecieverName;
            string GroupName;
            if (isGroup == "true")
            {
                var groupChat = await _unitOfWork.GroupChats.Get(u => u.Id == Guid.Parse(RecieverId));
                RecieverName = groupChat.Name;
            }
            else
            {
                var recieverUser = await _userManager.Users.FirstOrDefaultAsync(e => e.Id == Guid.Parse(RecieverId));
                RecieverName = recieverUser.Email;
            }

            GroupName = GetGroupName(Context.User.GetUserName(), RecieverName, isGroup);

            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName); //Add Connection Id to Group

            List<MessageResponse> messages;


            if (isGroup == "true")
            {
                messages = await _messageService.GetGroupMessages(Guid.Parse(RecieverId));

            }
            else
            {
                messages = await _messageService.GetMessages(Guid.Parse(RecieverId), Context.User.GetUserId());

            }


            await Clients.Group(GroupName).SendAsync("UserMessages", messages);
            await AddConnectionToGroup(Context.User.GetUserName(), GroupName);

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Sending messages between between two users
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public async Task SendMessage(MessageRequest messageRequest)
        {
            if (Context.User.GetUserId() == messageRequest.RecieverId) throw new HubException("Something is wrong");



            string RecieverName;
            if (messageRequest.isGroup == true)
            {
                var groupChat = await _unitOfWork.GroupChats.Get(u => u.Id == messageRequest.RecieverId);
                RecieverName = groupChat.Name;
            }
            else
            {
                var recieverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == messageRequest.RecieverId); await _userManager.Users.FirstOrDefaultAsync(u => u.Id == messageRequest.RecieverId);
                RecieverName = recieverUser.Email;
            }

            var response = _mapper.MessageRequestToRespone(messageRequest);

            response.SenderId = Context.User.GetUserId();

            var GroupName = GetGroupName(Context.User.GetUserName(), RecieverName, messageRequest.isGroup ? "true" : "false");


            var group = await _unitOfWork.Groups.Get(u => u.Name == GroupName);
            await Clients.Group(GroupName).SendAsync("NewMessage", response);

            await _messageService.SendMessage(messageRequest, Context.User.GetUserId());
        }

        /// <summary>
        /// This is for getting the groupName from the connection id
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public async Task<string> GetGroupWithConnectionId(string connectionId)
        {
            var connection = await _unitOfWork.Connections.Get(u => u.ConnectionId == connectionId);
            if (connection == null) return string.Empty;
            return connection.GroupName;
        }

        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            var groupName = await GetGroupWithConnectionId(Context.ConnectionId);
            if (!string.IsNullOrEmpty(groupName))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }

            return base.OnDisconnectedAsync(exception);
        }




        /// <summary>
        /// This is for creating a group in the database and adding connections to it
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="recieverName"></param>
        /// <returns></returns>
        private async Task AddConnectionToGroup(string userName, string groupName)
        {
            Group? group = await _unitOfWork.Groups.Get(e => e.Name == groupName);
            if (group == null)
            {
                await _unitOfWork.Groups.Add(new Group { Name = groupName });
            }
            else
            {
                group.Connections.Add(new Connection(userName, Context.ConnectionId, groupName));
            }
            await _unitOfWork.Save();

        }

        /// <summary>
        /// This for creating a groupname using two users names
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="RecieverName"></param>
        /// <returns></returns>
        private string GetGroupName(string UserName, string RecieverName, string isGroup)
        {
            if (isGroup == "true") return $"{RemoveStrangeCharacters(RecieverName)}";

            int o = string.CompareOrdinal(UserName, RecieverName);

            if (o < 0)
            {
                return RemoveStrangeCharacters($"{RecieverName}-{UserName}");
            }

            return RemoveStrangeCharacters($"{UserName}-{RecieverName}");
        }

        private string RemoveStrangeCharacters(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, @"[^a-zA-Z0-9]", "");
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
