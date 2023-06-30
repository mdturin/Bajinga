using Core.Entities.Identity;

namespace Core.Interfaces.IServices;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
