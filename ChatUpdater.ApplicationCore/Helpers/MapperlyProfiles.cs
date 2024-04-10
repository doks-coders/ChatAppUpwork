using ChatUpdater.Models.Entities;
using ChatUpdater.Models.Requests;
using ChatUpdater.Models.Response;
using Riok.Mapperly.Abstractions;

namespace ChatUpdater.ApplicationCore.Helpers
{

    [Mapper]
    public partial class MessageMapper
    {
        public partial Message MessageRequestToMessage(MessageRequest message);

        public partial List<MessageResponse> MessageToMessageResponse(List<Message> messages);

        public partial MessageResponse MessageRequestToRespone(MessageRequest message);


        public partial GroupChat GroupChatRequestToGroupChar(CreateGroupChatRequest message);

        public partial List<GroupChatResponse> GroupChatToGroupChatResponse(List<GroupChat> groupchat);

        public partial List<UserResponse> ApplicationUserToUserResponse(List<ApplicationUser> users);

        public partial UserResponse ApplicationUserToUserResponse(ApplicationUser users);



    }
}
