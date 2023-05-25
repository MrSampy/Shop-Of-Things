using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
    {
        readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            return new ObjectResult(await userService.GetAllAsync());
        }

        // GET: api/user/id
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<UserModel>> Get(Guid id)
        {
            return new ObjectResult(await userService.GetByIdAsync(id));
        }

        // GET: api/user/statuses
        [HttpGet("statuses")]
        public async Task<ActionResult<IEnumerable<UserStatusModel>>> GetStatuses()
        {
            return new ObjectResult(await userService.GetAllUserStatusesAsync());
        }

        // Post: api/user
        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserModel model)
        {
            try
            {
                await userService.AddAsync(model);
            }
            catch (Exception ex) 
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/user/statuses
        [HttpPost("statuses")]
        public async Task<ActionResult> AddUserStatus([FromBody] UserStatusModel model)
        {
            try
            {
                await userService.AddUserStatusAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/user
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] UserModel model)
        {
            try
            {
                await userService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/user/statuses
        [HttpPut("statuses")]
        public async Task<ActionResult> UpdateUserStatus([FromBody] UserStatusModel model)
        {
            try
            {
                await userService.UpdatUserStatusAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/user
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                await userService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/user/statuses
        [HttpDelete("statuses/{id:Guid}")]
        public async Task<ActionResult> DeleteUserStatus(Guid id)
        {
            try
            {
                await userService.DeleteUserStatusAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }
    }
}
