namespace main.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using global::main.Models;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("pge/usuarios")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly HashSet<string> allowedRoles = new() { "Cliente", "Procurador" };

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Senha não pode ser vazia.");
            }

            if (!allowedRoles.Contains(request.Role))
            {
                return BadRequest("Role inválido. Role deve ser 'Cliente' ou 'Procurador'.");
            }

          
            if (request.Role == "Procurador" && string.IsNullOrEmpty(request.OAB))
            {
                return BadRequest("OAB é obrigatória para Procurador.");
            }

            try
            {
                
                var user = new User
                {
                    Username = request.Username,
                    Password = request.Password,
                    Role = request.Role
                };

               
                _userService.AddUser(request);

                return Ok("Usuário registrado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao registrar usuário: {ex.Message}");
            }
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            var user = _userService.Authenticate(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            var token = generateJwt(user);

            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpPut("atualizar-credenciais")]
        public IActionResult UpdateCredentials([FromBody] UpdateCredentialsRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dados fornecidos estão inválidos.");
            }

            try
            {
                _userService.UpdateCredentials(request.Username, request.OldPassword, request.NewPassword);
                return Ok("Credenciais atualizadas com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar credenciais: {ex.Message}");
            }
        }


        private string generateJwt(User user)
        {
            var key = Encoding.ASCII.GetBytes("8d99dX1fZPqfD56Tkcj3pZdTdzdfsdfsdfdffwefsdcsrggwsfdsa");
            var Skey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(Skey,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                   issuer: "seu_emissor",
                   audience: "seu_publico",
                   claims: claims,
                   expires: DateTime.UtcNow.AddHours(1),
                   signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            
           
        }

       
        public class UserRegisterRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public string OAB { get; set; } 
            public string Nome { get; set; }
        }

        public class UserLoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class UpdateCredentialsRequest
        {
            public string Username { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
