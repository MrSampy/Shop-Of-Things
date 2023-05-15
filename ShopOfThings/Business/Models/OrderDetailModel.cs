using Data.Entities;
using System;
using System.Collections.Generic;
namespace Business.Models
{
    public class OrderDetailModel
    {
        public int Id { set; get; }

        public int ProductId { set; get; }

        public int OrderId { set; get; }

        public decimal Quantity { set; get; }

        public decimal? UnitPrice { set; get; }
    }
}
