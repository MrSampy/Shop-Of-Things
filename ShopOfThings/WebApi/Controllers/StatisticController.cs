using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "statistic")]
    public class StatisticController
    {
        readonly IStatisticService statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }

        // Post: api/statistic/avproductcategory/productCategoryId
        [HttpPost("avproductcategory/{productCategoryId:Guid}")]
        public async Task<ActionResult<decimal>> GetAverageOfProductCategory(Guid productCategoryId)
        {
            decimal result;
            try
            {
                result = await statisticService.GetAverageOfProductCategory(productCategoryId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return new ObjectResult(result);
        }

        // Post: api/statistic/incomeofcategoryperiod/productCategoryId
        [HttpPost("incomeofcategoryperiod")]
        public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod([FromBody] ProductsInPeriodModel productsInPeriodModel)
        {
            decimal result;
            try 
            {
                result = await statisticService.GetIncomeOfCategoryInPeriod(productsInPeriodModel.ProductCategoryId, productsInPeriodModel.StartDate, productsInPeriodModel.EndDate);
            }
            catch (Exception ex) 
            {
                return new BadRequestObjectResult(ex.Message);            
            }
            return new ObjectResult(result);
        }

        // GET: api/statistic/mostactiveuser
        [HttpGet("mostactiveuser")]
        public async Task<ActionResult<ActiveUsersModel>> GetMostActtiveUsers()
        {
            return new ObjectResult(await statisticService.GetMostActtiveUsers());
        }

        // GET: api/statistic/mostpopularproducts
        [HttpGet("mostpopularproducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts()
        {
            return new ObjectResult(await statisticService.GetMostPopularProducts());
        }

        // GET: api/statistic/productincategories
        [HttpGet("productincategories")]
        public async Task<ActionResult<List<ProductCategoryNumberModel>>> GetNumberOfProductsInCategories()
        {
            return new ObjectResult(await statisticService.GetNumberOfProductsInCategories());
        }

        // GET: api/statistic/agecategories
        [HttpGet("agecategories")]
        public async Task<ActionResult<List<UserAgeCategoryModel>>> GetNumberOfUsersOfEveryAgeCategory()
        {
            return new ObjectResult(await statisticService.GetNumberOfUsersOfEveryAgeCategory());
        }
    }
}
