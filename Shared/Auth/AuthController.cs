using Microsoft.AspNetCore.Mvc;

namespace Shared.Auth
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtService;

        public AuthController(JwtTokenService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Endpoint de login simples (não usa banco, apenas mock para teste).
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Usuário e senha fixos (apenas exemplo)
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtService.GenerateToken(request.Username, "Admin");
                return Ok(new { token });
            }

            return Unauthorized("Usuário ou senha inválidos");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
