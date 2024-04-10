using Microsoft.AspNetCore.Identity;

namespace ChatUpdater.Models.Entities
{
    /// <summary>
    /// This is the User entity for our Application. It is used for storing the content of our 
    /// user in the database
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<AppUserRole> AppUserRoles { get; set; }

        public string ProfilePicture { get; set; } = "";
        public List<GroupChat> MyGroups { get; set; } = new();
        public List<GCContacts> GroupContacts { get; set; }
    }
}
