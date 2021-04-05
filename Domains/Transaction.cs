using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingDiscount.Domains
{
    public class Transaction
    {
        public string Date { get; set; }
        public string SizeCode { get; set; }
        public string CarrierCode { get; set; }
        public ShippingPrice ShippingPrice { get; set; }
    }
}
