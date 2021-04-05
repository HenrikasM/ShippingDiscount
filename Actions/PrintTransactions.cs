using ShippingDiscount.DataImports;
using ShippingDiscount.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingDiscount.Actions
{
    public class PrintTransactions
    {
        readonly PopulateData PopulateData = new PopulateData();
        public void PrintTransaction(List<Transaction> Transactions)
        {
            foreach (Transaction transaction in Transactions)
            {
                Console.WriteLine(
                    transaction.Date + " " + (IsValid(transaction.SizeCode, transaction.CarrierCode) ?
                    transaction.SizeCode + " " + transaction.CarrierCode +
                    string.Format("{0:0.00}", transaction.ShippingPrice.Discount.ReducedPrice) + " " +
                    (transaction.ShippingPrice.Discount.DiscountedAmount != 0 ?
                    string.Format("{0:0.00}", transaction.ShippingPrice.Discount.DiscountedAmount) : " - ") : "Ignore")
                    );
            }
        }
        public bool IsValid(string size, string carrier)
        {
            if (PopulateData.AvaibleProviders().Contains(carrier) && PopulateData.AvaibleSizes().Contains(size))
                return true;
            else
                return false;
        }
    }
}
