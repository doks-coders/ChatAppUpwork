using Microsoft.AspNetCore.Identity;

namespace ChatUpdater.Models.Entities
{
    /// <summary>
    /// This is the AppRole entity for our Application
    /// </summary>
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> AppUserRoles { get; set; }
    }
}
