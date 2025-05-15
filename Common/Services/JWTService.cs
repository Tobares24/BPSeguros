using Common.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common.Services
{
    public class JWTService
    {
        private readonly ILogger<JWTService> _logger;

        public JWTService(ILogger<JWTService> logger)
        {
            _logger=logger;
        }

        public string GenerarToken(UsuarioEntity usuario)
        {
            try
            {
                TimeSpan expiration = TimeSpan.FromHours(int.Parse(Environment.GetEnvironmentVariable("EXPIRE_HOURS") ?? "12"));

                string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY")!;
                string issuer = Environment.GetEnvironmentVariable("ISSUER")!;
                string audience = Environment.GetEnvironmentVariable("AUDIENCE")!;

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", usuario.Id.ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    expires: DateTime.Now.Add(expiration),
                    claims: claims,
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError("Excepción: {1}", ex.ToString());
                string msg = "Ocurrió un error al intentar obtener el token de autenticación.";
                throw new BPSegurosException(500, msg, ex);
            }
        }
    }
}
