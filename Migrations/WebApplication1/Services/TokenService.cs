
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
            _issuer = jwtSection["Issuer"];
            _audience = jwtSection["Audience"];
        }
        public string GenerateToken(User user)
        {
            //string token = string.Empty;
            var claims = new List<Claim>(){
               new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials,
                Issuer = _issuer,
                Audience = _audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
