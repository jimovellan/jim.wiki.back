using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jim.wiki.back.infrastructure.Autentication.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly JWTConfiguration jwtConfiguration;

        public UserDataService(IHttpContextAccessor httpContext,IOptions<JWTConfiguration> jwtConfiguration)
        {
            this.httpContext = httpContext;
            this.jwtConfiguration = jwtConfiguration.Value;
        }
        public IUserData GetUser()
        {
            var claims = httpContext.HttpContext.User.Claims.Select(S => new { Tipo = S.Type, Value = S.Value });

            return new UserData()
            {
                Name = claims.FirstOrDefault(f => f.Tipo == ClaimTypes.Name).Value ?? "Annonimous",
                Email = claims.FirstOrDefault(f => f.Tipo == ClaimTypes.Email).Value ?? "No Email",
                IP = httpContext?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0"

            };
        }

        public string GetToken(IUserData userData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userData.Name),
                    new Claim(ClaimTypes.Email, userData.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtConfiguration.MinutesExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
