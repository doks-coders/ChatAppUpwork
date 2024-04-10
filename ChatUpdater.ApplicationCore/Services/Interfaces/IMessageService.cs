using ChatUpdater.ApplicationCore.Helpers;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;

namespace ChatUpdater.ApplicationCore.Services.Interfaces
{
    public interface IMessageService
    {
        /// <summary>
        /// This is used for getting all the messages between two users, from the database
        /// </summary>
        /// <param name="recieverId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MessageResponse>> GetMessages(Guid recieverId, Guid userId);

        /// <summary>
        /// This is used for storing the messages the database
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResponseModal<bool>> SendMessage(MessageRequest messageRequest, Guid userId);


        /// <summary>
        /// This is used for getting group chat messages
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<MessageResponse>> GetGroupMessages(Guid groupId);


        Task<ApiResponseModal<List<MessageResponse>>> GetChatMessages(Guid recieverId, Guid userId, bool isGroup);


    }
}
