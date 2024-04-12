using System.ComponentModel.DataAnnotations.Schema;

namespace ChatUpdater.Models.Entities
{
    public class GroupChat : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public bool IsPublic { get; set; }

        [ForeignKey(nameof(AdminUser))]
        public Guid AdminId { get; set; }
        public ApplicationUser? AdminUser { get; set; }


        public List<GCContacts> GroupContacts { get; set; }
    }
}
