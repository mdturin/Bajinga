using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions;

public static class UserManagerExtension
{
    public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> input, ClaimsPrincipal user)
    {
        var email = user.RetrieveEmailFromPrincipal();
        return await input.Users
            .Include(x => x.Address)
            .SingleOrDefaultAsync(x => x.Email == email);
    }
}
