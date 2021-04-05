using ShippingDiscount.Domains;
using System.Collections.Generic;

namespace ShippingDiscount.DataImports
{
    //Populate lists with ShippingPrices, Avaible Sizes/Carriers
    public class PopulateData
    {
        public List<ShippingPrice> SetShippingPrice()
        {

            return new List<ShippingPrice>
            {
                new ShippingPrice
                {
                    Provider = "LP",
                    PackageSize = "S",
                    Price = 1.50

                },
                new ShippingPrice
                {
                    Provider = "LP",
                    PackageSize = "M",
                    Price = 4.90
                },
                new ShippingPrice
                {
                    Provider = "LP",
                    PackageSize = "L",
                    Price = 6.90
                },
                new ShippingPrice
                {
                    Provider = "MR",
                    PackageSize = "S",
                    Price = 2
                },
                new ShippingPrice
                {
                    Provider = "MR",
                    PackageSize = "M",
                    Price = 3
                },
                new ShippingPrice
                {
                    Provider = "MR",
                    PackageSize = "L",
                    Price = 4
                }
            };
        }
        public List<string> AvaibleSizes()
        {
            return new List<string>
            {
                "S",
                "M",
                "L"
            };
        }
        public List<string> AvaibleProviders()
        {
            return new List<string>
            {
                "LP",
                "MR"
            };
        }
    }
}
