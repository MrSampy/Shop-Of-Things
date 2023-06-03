namespace Business.Models
{
    public class OrderPriceModel
    {
        public decimal FullPrice { get; set; }
        public IEnumerable<OrderDetailPriceModel> OrderDetailPrices {set; get;}
    }
}
