using ShippingDiscount.DataImports;
using ShippingDiscount.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingDiscount.Actions.DiscountActions
{
    public class AssignShippingDiscount
    {
        public int Lcount { get; set; }
        public double DiscountCount { get; set; }
        public string CurrentMonth { get; set; }
        public bool LimitExceeded { get; set; }

        readonly List<ShippingPrice> shippingPrices = new List<ShippingPrice>();
        public AssignShippingDiscount()
        {
            shippingPrices = new PopulateData().SetShippingPrice();
            Lcount = 0;
            LimitExceeded = false;
        }
        public List<Transaction> AssignShippingDiscounts(List<Transaction> transactions)
        {

            foreach (Transaction transaction in transactions)
            {
                if (CurrentMonth == null || CurrentMonth != GetMonth(transaction.Date))
                {
                    CurrentMonth = GetMonth(transaction.Date);
                    LimitExceeded = false;
                    Lcount = 0;
                    DiscountCount = 0;
                }

                if (transaction.ShippingPrice != null)
                {
                    double reducedPrice = CountDiscount(transaction);
                    double discountedAmount = GetDifference(reducedPrice, transaction.ShippingPrice.Price);

                    transaction.ShippingPrice.Discount = new ShippingPriceDiscount
                    {
                        ReducedPrice = reducedPrice,
                        DiscountedAmount = discountedAmount
                    };
                    if (!LimitExceeded)
                        DiscountCount += transaction.ShippingPrice.Discount.DiscountedAmount;
                }
            }

            return transactions;
        }
        public double CountDiscount(Transaction transaction)
        {
            double reducedPrice = transaction.ShippingPrice.Price;

            //Rule 1
            if (transaction.SizeCode == "S")
            {
                var sPrices = shippingPrices.FindAll(x => x.PackageSize == "S");
                reducedPrice = sPrices.Min(x => x.Price);
            }
            //Rule 2
            else if (transaction.SizeCode == "L" && transaction.CarrierCode == "LP")
            {
                Lcount += 1;
                if (Lcount == 3)
                {
                    reducedPrice = 0;
                }
            }

            double discountAmount = transaction.ShippingPrice.Price - reducedPrice;

            //Rule 3
            if (discountAmount > 10 - DiscountCount && !LimitExceeded)
            {
                LimitExceeded = true;
                reducedPrice = transaction.ShippingPrice.Price - (10 - DiscountCount);
            }
            return reducedPrice;
        }
        public double GetDifference(double reducedPrice, double basePrice)
        {
            return basePrice - reducedPrice;
        }
        public string GetMonth(string date)
        {
            var month = date.Split("-".ToArray());
            return month[1];
        }
    }
}
