using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Business.Models
{
    public class ProductModel
    {
        public int Id { set; get; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public int StorageTypeId { get; set; }
        public string StorageTypeName { get; set; }
        public decimal Amount { get; set; }

    }
}
