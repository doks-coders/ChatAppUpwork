namespace ChatUpdater.Models.Requests
{
    /// <summary>
    /// This is the request used for sending a message to a User. 
    /// It will be sent from the FrontEnd
    /// </summary>
    public class MessageRequest
    {
        public string Content { get; set; }
        public bool isGroup { get; set; } //true, false
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Guid RecieverId { get; set; }
    }
}
