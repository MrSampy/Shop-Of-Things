﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Receipt
    {

        public int Id { get; set; }

        public User User { get; set; }
        
        public string ReceiptName { get; set; }

        public string ReceiptDescription { set; get; }
    }
}
