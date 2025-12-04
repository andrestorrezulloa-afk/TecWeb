using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;

namespace TecWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserSecurityService _userSecurityService;

        public TokenController(IConfiguration configuration,
            IUserSecurityService userSecurityService)
        {
            _configuration = configuration;
            _userSecurityService = userSecurityService;
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin userLogin)
        {
            // Si es un usuario válido
            var validation = await IsValidUser(userLogin);
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new { token });
            }

            return NotFound();
        }

        private async Task<(bool, UserSecurity)> IsValidUser(UserLogin login)
        {
            var user = await _userSecurityService.GetLoginByCredentials(login);
            return (user != null, user);
        }

        private string GenerateToken(UserSecurity userSecurity)
        {
            // Header
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims (Cuerpo)
            var claims = new[]
            {
                new Claim("Login", userSecurity.Login),
                new Claim("Name", userSecurity.FullName),
                new Claim(ClaimTypes.Role, userSecurity.UserRole.ToString()),
            };

            // Payload
            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(2)
            );

            // Generar el token JWT
            var token = new JwtSecurityToken(header, payload);

            // Serializar el token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}