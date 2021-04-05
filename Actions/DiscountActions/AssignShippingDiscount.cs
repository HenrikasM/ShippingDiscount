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
        //Apply discount to transaction
        public List<Transaction> AssignShippingDiscounts(List<Transaction> transactions)
        {

            foreach (Transaction transaction in transactions)
            {
                //Null values for new month
                if (CurrentMonth == null || CurrentMonth != GetMonth(transaction.Date))
                {
                    CurrentMonth = GetMonth(transaction.Date);
                    LimitExceeded = false;
                    Lcount = 0;
                    DiscountCount = 0;
                }
                //Check if transaction is valid
                if (transaction.ShippingPrice != null)
                {
                    //Calculate discount values
                    double reducedPrice = CountDiscount(transaction);
                    double discountedAmount = GetDifference(reducedPrice, transaction.ShippingPrice.Price);

                    //Assign values for discount
                    transaction.ShippingPrice.Discount = new ShippingPriceDiscount
                    {
                        ReducedPrice = reducedPrice,
                        DiscountedAmount = discountedAmount
                    };
                    //Count discounted amount for 10EU limit (rule3)
                    if (!LimitExceeded)
                        DiscountCount += transaction.ShippingPrice.Discount.DiscountedAmount;
                }
            }

            return transactions;
        }
        public double CountDiscount(Transaction transaction)
        {
            //Assign base price
            double reducedPrice = transaction.ShippingPrice.Price;

            //Rule 1. If Size is S, check for lowest price.
            if (transaction.SizeCode == "S")
            {
                var sPrices = shippingPrices.FindAll(x => x.PackageSize == "S");
                reducedPrice = sPrices.Min(x => x.Price);
            }
            //Rule 2. 3rd L && LP transaction is free (once a month).
            else if (transaction.SizeCode == "L" && transaction.CarrierCode == "LP")
            {
                Lcount += 1;
                if (Lcount == 3)
                {
                    reducedPrice = 0;
                }
            }

            //Calculate discounted amount
            double discountAmount = transaction.ShippingPrice.Price - reducedPrice;

            //Rule 3. Discount amount cant be bigger than 10EU(Month). 
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
        //Get transaction month
        public string GetMonth(string date)
        {
            var month = date.Split("-".ToArray());
            return month[1];
        }
    }
}
