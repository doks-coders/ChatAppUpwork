using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Extensions;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;

namespace ChatUpdater.Controllers
{
    [Authorize]
    public class MessageController : ParentController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MessageMapper _mapper;
        private readonly IMessageService _messageService;
        public MessageController(IUnitOfWork unitOfWork, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
            _messageService = messageService;
        }




        [HttpPost("Send-Message")]
        public async Task<ApiResponseModal<bool>> SendMessage([FromBody] MessageRequest messageRequest)
        =>await _messageService.SendMessage(messageRequest, User.GetUserId());
            
        


        [HttpGet("get-chatmessages/")]
        public async Task<ApiResponseModal<List<MessageResponse>>> GetMessage([FromQuery] Guid recieverId, bool isGroup)
        => await _messageService.GetChatMessages(recieverId,User.GetUserId(), isGroup);






    }

}