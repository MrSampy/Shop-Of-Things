using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
//using WebApi.Middleware;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "order")]
    public class OrderController
    {
        readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        // GET: api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> Get()
        {
            return new ObjectResult(await orderService.GetAllAsync());
        }

        // GET: api/order/status
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<OrderStatusModel>>> GetOrderStatuses()
        {
            return new ObjectResult(await orderService.GetAlldOrderStatusesAsync());
        }

        // GET: api/order/id
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<OrderModel>> Get(Guid id)
        {
            return new ObjectResult(await orderService.GetByIdAsync(id));
        }

        // GET: api/order/id/fullPrice
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{id:Guid}/fullPrice")]
        public async Task<ActionResult<OrderPriceModel>> GetFullPrice(Guid id)
        {
            return new ObjectResult(await orderService.GetOrderFullPrice(id));
        }

        // GET: api/order/id/details
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{id:Guid}/details")]
        public async Task<ActionResult<OrderModel>> GetOrderDetails(Guid id)
        {
            return new ObjectResult(await orderService.GetOrderDetailsAsync(id));
        }

        // GET: api/order/period
        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrderByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return new ObjectResult(await orderService.GetOrdersByPeriodAsync(startDate,endDate));
        }

        // Post: api/order
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] OrderModel model)
        {
            try
            {
                await orderService.AddAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/order/status
        [HttpPost("status")]
        public async Task<ActionResult> AddStatus([FromBody] OrderStatusModel model)
        {
            try
            {
                await orderService.AddOrderStatusAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/order/id/products/add/productId/quantity
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("{id:Guid}/products/add/{productId:Guid}/{quantity:decimal}")]
        public async Task<ActionResult> AddProductToOrder(Guid id, Guid productId, decimal quantity)
        {
            try
            {
                await orderService.AddProductAsync(id, productId, quantity);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/order/id/products/remove/productId/quantity
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("{id:Guid}/products/remove/{productId:Guid}/{quantity:decimal}")]
        public async Task<ActionResult> RemoveQuantityOfProductFromOrder(Guid id, Guid productId, decimal quantity)
        {
            try
            {
                await orderService.RemoveProductAsync(id, productId, quantity);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/order
        [Authorize(Roles = "Admin,Customer")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] OrderModel model)
        {
            try
            {
                await orderService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/order/status
        [HttpPut("status")]
        public async Task<ActionResult> UpdateOrderStatus([FromBody] OrderStatusModel model)
        {
            try
            {
                await orderService.UpdatOrderStatusAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/order/id/changeStatus/statusId
        [HttpPut("{id:Guid}/changeStatus/{statusId:Guid}")]
        public async Task<ActionResult> ChangeOrderStatus(Guid id, Guid statusId)
        {
            try
            {
                await orderService.ChangeOrderStatus(id, statusId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/order/id
        [Authorize(Roles = "Admin,Customer")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await orderService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/order/status/id
        [HttpDelete("status/{id:Guid}")]
        public async Task<ActionResult> DeleteOrderStatus(Guid id)
        {
            try
            {
                await orderService.DeleteOrderStatusAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/order/id/product/remove/productId
        [Authorize(Roles = "Admin,Customer")]
        [HttpDelete("{id:Guid}/products/remove/{productId:Guid}")]
        public async Task<ActionResult> RemoveProductFromOrder(Guid id, Guid productId)
        {
            try
            {
                await orderService.RemoveProductByIdAsync(id, productId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }
    }
}
