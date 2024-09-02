
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
