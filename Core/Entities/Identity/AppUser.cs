using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public IdentityAddress Address { get; set; }
}