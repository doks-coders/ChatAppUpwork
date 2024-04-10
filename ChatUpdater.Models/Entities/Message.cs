using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatUpdater.Models.Entities
{
    /// <summary>
    /// This is the message entity. It creates the tables for storing the message data
    /// </summary>
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public bool isGroup { get; set; } = false;
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
        [NotMapped] public string UserName { get; set; }


        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationUser? Sender { get; set; }

        public DateTime DateRead { get; set; }
    }
}
