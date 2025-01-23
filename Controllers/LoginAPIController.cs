using APIRestIndotInventarioMovil.DTO;
using APIRestIndotInventarioMovil.Models;
using APIRestIndotInventarioMovil.Services;
using APIRestIndotInventarioMovil.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIRestIndotInventarioMovil.Controllers
{
    [Route("apiindot/[controller]")]
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        private readonly IServicioSQL _servicioSQL;
        private readonly IConfiguration _configuration;
        public LoginAPIController(IServicioSQL servicioSQL, IConfiguration configuration)
        {
            _servicioSQL = servicioSQL; 
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioApiDTO>> Login(LoginAPI usuarioLogin)
        {
            UsuarioSQLServer Usuario = null;
            Usuario = await AutenticarUsuarioAsync(usuarioLogin);
            if (Usuario == null)
                throw new Exception("Credenciales no válidas");
            else
                Usuario = GenerarTokenJWT(Usuario);

                return Usuario.convertirDTO();
        }

        private async Task<UsuarioSQLServer> AutenticarUsuarioAsync(LoginAPI usuarioLogin)
        {
            UsuarioSQLServer usuarioAPI = await _servicioSQL.DameUsuarioSQLServer(usuarioLogin); //Login con base datos local SQL Server
            
            return usuarioAPI;
        }

        private UsuarioSQLServer GenerarTokenJWT(UsuarioSQLServer usuarioSQLServer)
        {
            // Cabecera
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JWT:ClaveSecreta"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256 // Establecer cifrado en función
                                                                         // de la longitud de clave secreta en appSettings.json
                );
            var _Header = new JwtHeader(_signingCredentials);
            // Claims
            var _Claims = new[] {
                new Claim("email", usuarioSQLServer.Email),
                new Claim("usuario", usuarioSQLServer.Usuario),
                new Claim(JwtRegisteredClaimNames.Email, usuarioSQLServer.Email),
            };

            //Payload
            var _Payload = new JwtPayload(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(60) //Minutos de caducidad del Token
                );

            // Token
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            usuarioSQLServer.Token = new JwtSecurityTokenHandler().WriteToken(_Token);

            return usuarioSQLServer;
        }
    }
}
