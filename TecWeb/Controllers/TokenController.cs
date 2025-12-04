using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
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
        private readonly IPasswordService _passwordService;

        public TokenController(
            IConfiguration configuration,
            IUserSecurityService userSecurityService,
            IPasswordService passwordService)
        {
            _configuration = configuration;
            _userSecurityService = userSecurityService;
            _passwordService = passwordService;
        }

        // ENDPOINT PARA OBTENER TOKEN
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authentication(UserLogin userLogin)
        {
            var validation = await IsValidUser(userLogin);
            if (validation.Item1 && validation.Item2 != null)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        // ============================================
        // ENDPOINTS DE DIAGNÓSTICO (NUEVOS)
        // ============================================

        [HttpGet("TestConeccion")]  // Nota: Con dos C como en la guía
        [AllowAnonymous]
        public IActionResult TestConeccion()
        {
            try
            {
                var result = new
                {
                    ConnectionMySql = _configuration["ConnectionStrings:ConnectionMySql"] ?? "My SQL NO CONFIGURADO",
                    ConnectionSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"] ?? _configuration["ConnectionStrings:connectionDB"] ?? "SQL SERVER NO CONFIGURADO",
                    Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "NO CONFIGURADO",
                    Timestamp = DateTime.UtcNow
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        [HttpGet("TestConexion")]  // También mantener el anterior por compatibilidad
        [AllowAnonymous]
        public IActionResult TestConexion()
        {
            try
            {
                var result = new
                {
                    ConnectionDB = _configuration["ConnectionStrings:connectionDB"] != null
                        ? "CONFIGURADO (valor oculto por seguridad)"
                        : "NO CONFIGURADO",
                    Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "NO CONFIGURADO",
                    TimeStamp = DateTime.UtcNow,
                    MachineName = Environment.MachineName
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        [HttpGet("Config")]
        [AllowAnonymous]
        public IActionResult GetConfig()
        {
            try
            {
                var connectionStringMySql = _configuration["ConnectionStrings:ConnectionMySql"];
                var connectionStringSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"];
                var connectionStringDB = _configuration["ConnectionStrings:connectionDB"];

                var result = new
                {
                    connectionStringMySql = connectionStringMySql ?? "My SQL NO CONFIGURADO",
                    connectionStringSqlServer = connectionStringSqlServer ?? connectionStringDB ?? "SQL SERVER NO CONFIGURADO",
                    AllConnectionStrings = _configuration.GetSection("ConnectionStrings").GetChildren()
                        .Select(x => new { Key = x.Key, Value = x.Key == "connectionDB" ? "*****OCULTO POR SEGURIDAD*****" : x.Value })
                        .ToList(),
                    Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "ASPNETCORE_ENVIRONMENT NO CONFIGURADO",
                    Authentication = _configuration.GetSection("Authentication").GetChildren()
                        .Select(x => new { Key = x.Key, Value = x.Key == "SecretKey" ? "*****OCULTO POR SEGURIDAD*****" : x.Value })
                        .ToList(),
                    PasswordOptions = _configuration.GetSection("PasswordOptions").GetChildren()
                        .Select(x => new { Key = x.Key, Value = x.Value })
                        .ToList(),
                    Timestamp = DateTime.UtcNow,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name,
                    Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        // ============================================
        // ENDPOINT TEMPORAL PARA GENERAR HASH (DESARROLLO)
        // ============================================

        [HttpPost("generate-hash")]
        [AllowAnonymous]
        public IActionResult GenerateHash([FromBody] string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                    return BadRequest(new { error = "La contraseña no puede estar vacía" });

                var hash = _passwordService.Hash(password);
                return Ok(new
                {
                    Password = password,
                    Hash = hash,
                    Message = "Usa este hash para insertar en la tabla UserSecurity",
                    Instructions = "Ejecuta en SQL: INSERT INTO UserSecurity (Login, PasswordHash, FullName, UserRole) VALUES ('admin', '" + hash + "', 'Administrador', 'Administrator')"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("verify-hash")]
        [AllowAnonymous]
        public IActionResult VerifyHash([FromBody] HashVerificationRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Hash) || string.IsNullOrEmpty(request.Password))
                    return BadRequest(new { error = "Hash y contraseña son requeridos" });

                var isValid = _passwordService.Check(request.Hash, request.Password);
                return Ok(new
                {
                    Hash = request.Hash,
                    Password = request.Password,
                    IsValid = isValid,
                    Message = isValid ? "Hash válido" : "Hash inválido"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ============================================
        // MÉTODOS PRIVADOS
        // ============================================

        private async Task<(bool, UserSecurity?)> IsValidUser(UserLogin login)
        {
            var user = await _userSecurityService.GetLoginByCredentials(login);
            if (user == null)
                return (false, null);

            // Verificar contraseña usando PasswordService
            var isValid = _passwordService.Check(user.PasswordHash, login.Password);
            return (isValid, user);
        }

        private string GenerateToken(UserSecurity userSecurity)
        {
            var expireMinutes = int.TryParse(_configuration["Authentication:ExpireMinutes"], out var minutes)
                ? minutes
                : 60;

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));

            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Login", userSecurity.Login),
                new Claim("Name", userSecurity.FullName),
                new Claim(ClaimTypes.Role, userSecurity.UserRole.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim("UserId", userSecurity.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Clase auxiliar para verificación de hash
    public class HashVerificationRequest
    {
        public string Hash { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}