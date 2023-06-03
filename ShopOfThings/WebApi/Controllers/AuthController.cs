using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
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
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, userService.GetUserRoleByUserNickName(userName).Result.UserRoleName) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // POST: api/auth/login
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = "user")]
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
