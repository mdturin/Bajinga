using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class IdentityDbContextSeed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (userManager.Users.Any()) return;

        var user = new AppUser
        {
            DisplayName = "Bob",
            Email = "bob@test.com",
            UserName = "bob@test.com",
            Address = new IdentityAddress
            {
                FirstName = "Bob",
                LastName = "Bobbity",
                Street = "10 The Street",
                City = "New York",
                State = "NY",
                ZipCode = "90210"
            }
        };

        await userManager.CreateAsync(user, "Pa$$w0rd");
    }
}
