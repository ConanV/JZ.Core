using IdentityServer4.Test;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer4AspNetIdentity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        private readonly TestUserStore _users;
        public ApplicationUser(TestUserStore users = null)
        {
            _users = users ?? new TestUserStore(TestUsers.Users);
            
        }
            
    }
}
