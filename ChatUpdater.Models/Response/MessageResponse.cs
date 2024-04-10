namespace ChatUpdater.Models.Response
{
    /// <summary>
    /// This is the response used for sending a message. 
    /// It will be sent to the FrontEnd
    /// </summary>
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
        public DateTime DateRead { get; set; }
    }
}
