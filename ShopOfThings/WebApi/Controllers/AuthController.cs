using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController
    {
        readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        // POST: api/auth/login
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

            return new OkObjectResult(result);
        }
    }
}
