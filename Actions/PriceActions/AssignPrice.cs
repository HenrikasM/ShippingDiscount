using ShippingDiscount.DataImports;
using ShippingDiscount.Domains;
using System;

namespace ShippingDiscount.Actions.PriceActions
{
    public class AssignPrice
    {
        public ShippingPrice AssignShippingPrice(string CarrierCode, string SizeCode)
        {
            try
            {
                return new PopulateData().SetShippingPrice().Find(x => (x.Provider == CarrierCode) &&
                (x.PackageSize == SizeCode));
            }
            catch (InvalidOperationException err)
            {
                Console.WriteLine("Shipping price not found: " + err);
                return null;
            }
        }
    }
}
