namespace ChatUpdater.Models.Response
{
    public class GroupChatResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public Guid AdminId { get; set; }
    }
}
