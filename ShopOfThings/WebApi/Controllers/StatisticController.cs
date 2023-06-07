using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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

        // GET: api/statistic/avproductcategory/productCategoryId
        [HttpGet("avproductcategory/{productCategoryId:Guid}")]
        public async Task<ActionResult<decimal>> GetAverageOfProductCategory(Guid productCategoryId)
        {
            return new ObjectResult(await statisticService.GetAverageOfProductCategory(productCategoryId));
        }

        // GET: api/statistic/incomeofcategoryperiod/productCategoryId
        [HttpGet("incomeofcategoryperiod/{productCategoryId:Guid}")]
        public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod(Guid productCategoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return new ObjectResult(await statisticService.GetIncomeOfCategoryInPeriod(productCategoryId,startDate,endDate));
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
