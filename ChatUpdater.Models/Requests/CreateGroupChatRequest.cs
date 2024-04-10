namespace ChatUpdater.Models.Requests
{
    public class CreateGroupChatRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
    }
}
