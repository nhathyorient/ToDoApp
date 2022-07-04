using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Identity.Extensions
{
    public static class UserManagerExtension
    {
        public static Task<TUser> GetUserEnsureNotNullAsync<TUser>(this UserManager<TUser> userManager, ClaimsPrincipal userClaimsPrincipal) where TUser : IdentityUser
        {
            var user = userManager.GetUserAsync(userClaimsPrincipal);

            if (user == null) throw new Exception("User not found!");

            return user!;
        }
    }
}
