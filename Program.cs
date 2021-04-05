using NUnitLite;
using ShippingDiscount.Actions;
using ShippingDiscount.Actions.DiscountActions;
using ShippingDiscount.DataImports;
using System;
using System.Reflection;

namespace ShippingDiscount
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read file
            FileParse fileParse = new FileParse();
            Console.Write("File name: ");
            var Text = fileParse.ReadFile(Console.ReadLine());

            //Create Transactions
            var Transactions = fileParse.ParseDataToTransactions(Text);

            //Apply discount to Transactions
            var discountedTransactions = new AssignShippingDiscount().AssignShippingDiscounts(Transactions);

            //Print Transaction
            new PrintTransactions().PrintTransaction(discountedTransactions);

            //Run Tests
            Console.WriteLine("Do you want to run tests?(Y/N)");
            if (Console.ReadLine() == "Y")
            {
                new AutoRun(Assembly.GetExecutingAssembly())
                       .Execute(new string[] { "/test:ShippingDiscount.Tests.ShippingDiscountTest" });
            }
        }
    }
}
