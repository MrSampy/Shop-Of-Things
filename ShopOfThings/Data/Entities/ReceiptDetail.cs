﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ReceiptDetail
    {
        public int Id { get; set; }

        public Receipt Receipt {set; get;}

        public Product Product { get; set; }
    }
}
