﻿using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ReceiptModel>> Get(Guid id)
        {
            return new ObjectResult(await receiptService.GetByIdAsync(id));
        }

        // Post: api/receipt
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

        // Post: api/receipt/id/product/add/productId/amount
        [HttpPost("{id:Guid}/products/add/{productId:Guid}/{amount:decimal}")]
        public async Task<ActionResult> AddProductToReceipt(Guid receiptId, Guid productId, decimal amount)
        {
            try
            {
                await receiptService.AddProductAsync(receiptId,productId,amount);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/receipt/id/product/remove/productId/amount
        [HttpPost("{id:Guid}/products/remove/{productId:Guid}/{amount:decimal}")]
        public async Task<ActionResult> RemoveAmountOfProductFromReceipt(Guid receiptId, Guid productId, decimal amount)
        {
            try
            {
                await receiptService.AddProductAsync(receiptId, productId, amount);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/receipt/id
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
        [HttpGet("{id:Guid}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetDetails(Guid id)
        {
            return new ObjectResult(await receiptService.GetReceiptDetailsAsync(id));
        }

        // Post: api/receipt/id/product/remove/productId
        [HttpDelete("{id:Guid}/products/remove/{productId:Guid}")]
        public async Task<ActionResult> RemoveProductFromReceipt(Guid receiptId, Guid productId)
        {
            try
            {
                await receiptService.RemoveProductByIdAsync(receiptId, productId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/receipt
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
