using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingDiscount.Domains
{
    public class ShippingPrice
    {
        public string Provider { get; set; }
        public string PackageSize { get; set; }
        public double Price { get; set; }
        public ShippingPriceDiscount Discount { get; set; }

    }
}
