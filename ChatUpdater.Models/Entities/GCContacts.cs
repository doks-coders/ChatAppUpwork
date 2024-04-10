namespace ChatUpdater.Models.Entities
{
    public class GCContacts
    {
        public Guid Id { get; set; }

        public Guid AppGroupId { get; set; }
        public GroupChat AppGroupChat { get; set; }


        public Guid AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }

    }
}
