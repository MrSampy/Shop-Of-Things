using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController
    {
        readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        // Post: api/prdocut
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductModel model)
        {
            try
            {
                await productService.AddAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/prdocut/storageType
        [HttpPost("storageType")]
        public async Task<ActionResult> AddStorageType([FromBody] StorageTypeModel model)
        {
            try
            {
                await productService.AddStorageTypeAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Post: api/prdocut/category
        [HttpPost("category")]
        public async Task<ActionResult> AddProductCategory([FromBody] ProductCategoryModel model)
        {
            try
            {
                await productService.AddProductCategoryAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Get: api/prdocut
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get()
        {
            return new ObjectResult(await productService.GetAllAsync());
        }

        // Get: api/prdocut/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetWithFilter([FromQuery] ProductFilterSearchModel filter)
        {
            return new ObjectResult(await productService.GetByFilterAsync(filter));
        }

        // Get: api/prdocut/storageType
        [HttpGet("storageType")]
        public async Task<ActionResult<IEnumerable<StorageTypeModel>>> GetStorageTypes()
        {
            return new ObjectResult(await productService.GetAllStorageTypesAsync());
        }

        // Get: api/prdocut/category
        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetProductCategories()
        {
            return new ObjectResult(await productService.GetAllProductCategoriesAsync());
        }

        // Put: api/prdocut
        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            try
            {
                await productService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/prdocut/storageType
        [HttpPut("storageType")]
        public async Task<ActionResult> UpdateStorageType([FromBody] StorageTypeModel model)
        {
            try
            {
                await productService.UpdatStorageTypeAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Put: api/prdocut/category
        [HttpPut("category")]
        public async Task<ActionResult> UpdateProductCategory([FromBody] ProductCategoryModel model)
        {
            try
            {
                await productService.UpdatProductCategoryAsync(model);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/prdocut/id
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await productService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/prdocut/storageType/id
        [HttpDelete("storageType/{id:Guid}")]
        public async Task<ActionResult> DeleteStorageType(Guid id)
        {
            try
            {
                await productService.DeleteStorageTypeAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Delete: api/prdocut/category/id
        [HttpDelete("category/{id:Guid}")]
        public async Task<ActionResult> DeleteProductCategory(Guid id)
        {
            try
            {
                await productService.DeleteProductCategoryAsync(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkResult();
        }

        // Get: api/prdocut/id
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ProductModel>> Get(Guid id)
        {
            return new ObjectResult(await productService.GetByIdAsync(id));
        }
    }
}
