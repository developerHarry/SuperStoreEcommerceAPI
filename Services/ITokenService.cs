using SuperStoreEcommerceAPI.Models;

namespace SuperStoreEcommerceAPI.Services
{
    public interface ITokenService
    {
        (string token, DateTime expiresUtc) CreateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
