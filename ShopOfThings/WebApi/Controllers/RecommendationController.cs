using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[Authorize(Roles = "Admin,Customer")]
    [Authorize]
    [Route("api/user/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "recommendation")]
    public class RecommendationController
    {
        readonly IRecommendationService recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            this.recommendationService = recommendationService;
        }

        // GET: api/user/recommendation/id/products
        [HttpGet("{id:Guid}/products")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsByRecent(Guid id)
        {
            return new ObjectResult(await recommendationService.GetProductsByRecent(id));
        }

        // GET: api/user/recommendation/id/receipts
        [HttpGet("{id:Guid}/receipts")]
        public async Task<ActionResult<IEnumerable<RecommendationReceiptsModel>>> GetReceiptsByProducts(Guid id)
        {
            return new ObjectResult(await recommendationService.GetReceiptsByProducts(id));
        }
    }
}
