namespace Business.Models
{
    public class RecommendationReceiptsModel
    {
        public ReceiptModel Receipt { get; set; }
        public IEnumerable<ProductModel> MissingProducts { get; set; }

    }
}
