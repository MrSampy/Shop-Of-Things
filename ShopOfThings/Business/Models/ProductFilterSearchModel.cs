namespace Business.Models
{
    public class ProductFilterSearchModel
    {
        public int? CategoryId { set; get; }
        public int? MinPrice { set; get; }
        public int? MaxPrice { set; get; }
    }
}
