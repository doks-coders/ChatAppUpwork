using System.Security.Claims;

namespace ChatUpdater.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name);
        }

        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}


