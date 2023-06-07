using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController
    {
        private readonly IUserService userService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            this.userService = userService;
        }
        private string GenerateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var userRoleName = userService.GetUserRoleByUserNickName(userName).Result.UserRoleName;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, userRoleName) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // POST: api/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] LogInModel logInModel)
        {
            bool result;
            try
            {
                result = await userService.LogIn(logInModel.NickName,logInModel.Password);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            if (result) 
            {
                var tokenString = GenerateJwtToken(logInModel.NickName);
                return new OkObjectResult(new { token = tokenString});
            }
            return new UnauthorizedResult();
        }
    }
}
