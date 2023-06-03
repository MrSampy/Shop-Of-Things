using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "user")]

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

        // GET: api/user/roles
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<UserRoleModel>>> GetRolees()
        {
            return new ObjectResult(await userService.GetAllUserRolesAsync());
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

        // Post: api/user/roles
        [HttpPost("roles")]
        public async Task<ActionResult> AddUserRole([FromBody] UserRoleModel model)
        {
            try
            {
                await userService.AddUserRoleAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/user
        [Authorize(Roles = "Admin,Customer")]
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

        // Put: api/user/roles
        [HttpPut("roles")]
        public async Task<ActionResult> UpdateUserRole([FromBody] UserRoleModel model)
        {
            try
            {
                await userService.UpdatUserRoleAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/user/id
        [Authorize(Roles = "Admin,Customer")]
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

        // GET: api/user/id/receipts
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("user/{id:Guid}/receipts")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetReceiptsByUserId(Guid userId)
        {
            return new ObjectResult(await userService.GetReceiptsByUserId(userId));
        }

        // GET: api/user/id/products
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("user/{id:Guid}/products")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsByUserId(Guid userId)
        {
            return new ObjectResult(await userService.GetProductsByUserId(userId));
        }

        // GET: api/user/id/orders
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("user/{id:Guid}/orders")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrdersByUserId(Guid userId)
        {
            return new ObjectResult(await userService.GetOrdersByUserId(userId));
        }

        // Delete: api/user/roles/id
        [HttpDelete("roles/{id:Guid}")]
        public async Task<ActionResult> DeleteUserRole(Guid id)
        {
            try
            {
                await userService.DeleteUserRoleAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }
    }
}
