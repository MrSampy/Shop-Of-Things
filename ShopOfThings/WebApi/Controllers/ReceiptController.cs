using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "receipt")]
    public class ReceiptController
    {
        readonly IReceiptService receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }


        // GET: api/receipt
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            return new ObjectResult(await receiptService.GetAllAsync());
        }

        // GET: api/receipt/id
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ReceiptModel>> Get(Guid id)
        {
            return new ObjectResult(await receiptService.GetByIdAsync(id));
        }

        // Post: api/receipt
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost]
        public async Task<ActionResult> AddReceipt([FromBody] ReceiptModel model)
        {
            try
            {
                await receiptService.AddAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/receipt/id/products/add/productId/amount
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("{id:Guid}/products/add/{productId:Guid}/{amount:decimal}")]
        public async Task<ActionResult> AddProductToReceipt(Guid id, Guid productId, decimal amount)
        {
            try
            {
                await receiptService.AddProductAsync(id, productId,amount);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/receipt/id/products/remove/productId/amount
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("{id:Guid}/products/remove/{productId:Guid}/{amount:decimal}")]
        public async Task<ActionResult> RemoveAmountOfProductFromReceipt(Guid id, Guid productId, decimal amount)
        {
            try
            {
                await receiptService.RemoveProductAsync(id, productId, amount);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/receipt/id
        [Authorize(Roles = "Admin,Customer")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteReceipt(Guid id)
        {
            try
            {
                await receiptService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // GET: api/receipt/id/details
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{id:Guid}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetDetails(Guid id)
        {
            return new ObjectResult(await receiptService.GetReceiptDetailsAsync(id));
        }

        // Delete: api/receipt/id/product/remove/productId
        [Authorize(Roles = "Admin,Customer")]
        [HttpDelete("{id:Guid}/products/remove/{productId:Guid}")]
        public async Task<ActionResult> RemoveProductFromReceipt(Guid id, Guid productId)
        {
            try
            {
                await receiptService.RemoveProductByIdAsync(id, productId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/receipt
        [Authorize(Roles = "Admin,Customer")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ReceiptModel receipt) 
        {
            try
            {
                await receiptService.UpdateAsync(receipt);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/receipt/details
        [Authorize(Roles = "Admin,Customer")]
        [HttpPut("details")]
        public async Task<ActionResult> UpdateDetails([FromBody] ReceiptDetailModel receiptDetail)
        {
            try
            {
                await receiptService.UpdatReceiptDetailAsync(receiptDetail);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }
    }
}
