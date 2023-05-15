﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ReceiptDetail: BaseEntity
    {
        public int ReceiptId { set; get; }

        public Receipt? Receipt {set; get;}
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public decimal Amount { get; set; }

    }
}
