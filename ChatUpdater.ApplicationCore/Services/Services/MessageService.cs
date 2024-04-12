using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Infrastructure.Validators.Message;
using ChatUpdater.Models;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;

namespace ChatUpdater.ApplicationCore.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MessageMapper _mapper;
        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MessageMapper();
        }


        /// <summary>
        /// This is used for getting messages between two users
        /// </summary>
        /// <param name="recieverId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<MessageResponse>> GetMessages(Guid recieverId, Guid userId)
        {
            var messages = await _unitOfWork.Messages.GetAll(u =>
            u.RecieverId == recieverId
            &&
            u.SenderId == userId
            || //Both Sides
            u.SenderId == recieverId
            &&
            u.RecieverId == userId
            , includeProperties: "Sender"
            );
            messages = messages.OrderBy(e => e.DateCreated);
            var res = _mapper.MessageToMessageResponse(messages.ToList());
            return res;

        }

        /// <summary>
        /// This is used for getting messages sent between users and groups
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<List<MessageResponse>> GetGroupMessages(Guid groupId)
        {
            var messages = await _unitOfWork.Messages.GetAll(u =>
            u.RecieverId == groupId
            && u.isGroup == true,
            includeProperties: "Sender"
            );
            messages = messages.OrderBy(e => e.DateCreated).Select(u =>
            {
                u.UserName = u.Sender.UserName;
                return u;
            });

            var response = _mapper.MessageToMessageResponse(messages.ToList());
            return response;

        }


        /// <summary>
        /// This is used for sending messages to users or groups
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponseModal<bool>> SendMessage(MessageRequest messageRequest, Guid userId)
        {
            var messageRequestValidator = new MessageRequestValidator();
            var validation = await messageRequestValidator.ValidateAsync(messageRequest);
            if (!validation.IsValid) throw new ApiErrorException(validation.Errors);

            if (messageRequest.RecieverId == userId) throw new ApiErrorException(BaseErrorCodes.IdentityError);
            var message = _mapper.MessageRequestToMessage(messageRequest);
            message.SenderId = userId;
            await _unitOfWork.Messages.Add(message);
            if (await _unitOfWork.Save())
            {
                return await ApiResponseModal<bool>.SuccessAsync(true);
            }
            throw new ApiErrorException(BaseErrorCodes.DatabaseUnknownError);

        }

        public async Task<ApiResponseModal<List<MessageResponse>>> GetChatMessages(Guid recieverId, Guid userId, bool isGroup)
        {
            List<MessageResponse> messages = new();

            if (isGroup == true)
            {
                messages = await GetGroupMessages(recieverId);
            }
            else
            {
                messages = await GetMessages(recieverId, userId);
            }

            return await ApiResponseModal<List<MessageResponse>>.SuccessAsync(messages);

        }
    }
}
