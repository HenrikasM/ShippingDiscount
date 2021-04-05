using NUnit.Framework;
using ShippingDiscount.Actions.DiscountActions;
using ShippingDiscount.DataImports;
using ShippingDiscount.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingDiscount.Tests
{
    [TestFixture]
    public class ShippingDiscountTest
    {
        [Test]
        //Check if input.txt output is equal to correctOutput.txt
        public void TestCase1()
        {
            //Prepare
            var transactions = new List<Transaction>();
            FileParse fileParse = new FileParse();
            var parsedTransactions = fileParse.ReadFile("correctOutput.txt");
            string[] createdTransactions = parsedTransactions.Split("\n\n".ToCharArray());
            for (int y = 0; y < 20; y++)
            {
                string[] transaction = createdTransactions[y].Split(" ".ToCharArray());
                if (transaction.Length == 5)
                {
                    string DiscountedAmount = transaction[4].Trim();
                    if (DiscountedAmount == "-")
                        DiscountedAmount = "0";


                    transactions.Add(new Transaction
                    {
                        Date = transaction[0].Trim(),
                        SizeCode = transaction[1].Trim(),
                        CarrierCode = transaction[2].Trim(),
                        ShippingPrice = new ShippingPrice
                        {
                            PackageSize = "",
                            Provider = "",
                            Price = 0,
                            Discount = new ShippingPriceDiscount
                            {
                                ReducedPrice = Convert.ToDouble(transaction[3]),
                                DiscountedAmount = Convert.ToDouble(DiscountedAmount)
                            }
                        }
                    });
                }
                else if (transaction.Length < 5)
                {
                    transactions.Add(new Transaction
                    {
                        Date = transaction[0].ToString(),
                        SizeCode = transaction[1].ToString(),
                    });
                }
            }

            //Act
            var Text = fileParse.ReadFile("input.txt");
            var Transactions = fileParse.ParseDataToTransactions(Text);
            var correctTransactions = new AssignShippingDiscount().AssignShippingDiscounts(Transactions);

            //Assert
            for (int x = 0; x < 20; x++)
            {
                Assert.AreEqual(transactions[x].Date, correctTransactions[x].Date);
            }

        }

        //Create transaction and find shipping price
        [Test]
        public void TestCase2()
        {
            //Prepare
            var expected = new Transaction
            {
                Date = "2020-01-01",
                SizeCode = "S",
                CarrierCode = "LP",
                ShippingPrice = new ShippingPrice
                {
                    PackageSize = "S",
                    Provider = "LP",
                    Price = 1.50,
                    Discount = null
                }
            };

            //Act
            FileParse fileParse = new FileParse();
            string trasanctionString = "2020-01-01 S LP";
            var transactions = fileParse.ParseDataToTransactions(trasanctionString);

            //Assert
            Assert.AreEqual(expected.Date, transactions[0].Date);
            Assert.AreEqual(expected.CarrierCode, transactions[0].CarrierCode);
            Assert.AreEqual(expected.SizeCode, transactions[0].SizeCode);
            Assert.AreEqual(expected.ShippingPrice.Price, transactions[0].ShippingPrice.Price);
        }
        [Test]
        //Discount rule 1
        public void TestCase4()
        {
            //Prepare
            var transaction = new Transaction
            {
                Date = "2020-01-01",
                SizeCode = "S",
                CarrierCode = "MR",
                ShippingPrice = new ShippingPrice
                {
                    PackageSize = "S",
                    Provider = "MR",
                    Price = 2.00,
                    Discount = new ShippingPriceDiscount
                    {
                        DiscountedAmount = 0.50,
                        ReducedPrice = 1.50
                    }
                }
            };

            //Act
            FileParse fileParse = new FileParse();
            string trasanctionString = "2020-01-01 S MR";
            var transactions = fileParse.ParseDataToTransactions(trasanctionString);
            var discountedTransactions = new AssignShippingDiscount().AssignShippingDiscounts(transactions);

            //Assert
            Assert.AreEqual(transaction.ShippingPrice.Price, discountedTransactions[0].ShippingPrice.Price);
            Assert.AreEqual(transaction.ShippingPrice.Discount.ReducedPrice, discountedTransactions[0].ShippingPrice.Discount.ReducedPrice);
            Assert.AreEqual(transaction.ShippingPrice.Discount.DiscountedAmount, discountedTransactions[0].ShippingPrice.Discount.DiscountedAmount);
        }

        //Discount rule 2
        [Test]
        public void TestCase5()
        {
            //Prepare
            var transactions = new List<Transaction>();
            for (int x = 0; x < 4; x++)
            {
                transactions.Add(new Transaction
                {
                    Date = "2020-01-01",
                    SizeCode = "L",
                    CarrierCode = "LP",
                    ShippingPrice = new ShippingPrice
                    {
                        PackageSize = "L",
                        Provider = "LP",
                        Price = 6.90,
                        Discount = null
                    }
                }
            );
            }

            //Act
            var discountedTransaction = new AssignShippingDiscount().AssignShippingDiscounts(transactions);

            //Assert
            Assert.AreEqual(6.90, discountedTransaction[0].ShippingPrice.Discount.ReducedPrice);
            Assert.AreEqual(6.90, discountedTransaction[1].ShippingPrice.Discount.ReducedPrice);
            Assert.AreEqual(0, discountedTransaction[2].ShippingPrice.Discount.ReducedPrice);
            Assert.AreEqual(6.90, discountedTransaction[3].ShippingPrice.Discount.ReducedPrice);
        }
        //Discount rule 3
        [Test]
        public void TestCase6()
        {
            //Prepare
            var transactions = new List<Transaction>();
            for (int x = 0; x < 6; x++)
            {
                transactions.Add(new Transaction
                {
                    Date = "2020-01-01",
                    SizeCode = "S",
                    CarrierCode = "MR",
                    ShippingPrice = new ShippingPrice
                    {
                        PackageSize = "S",
                        Provider = "MR",
                        Price = 2.00,
                        Discount = null
                    }
                }
            );
            }
            for (int x = 0; x < 4; x++)
            {
                transactions.Add(new Transaction
                {
                    Date = "2020-01-01",
                    SizeCode = "L",
                    CarrierCode = "LP",
                    ShippingPrice = new ShippingPrice
                    {
                        PackageSize = "L",
                        Provider = "LP",
                        Price = 6.90,
                        Discount = null
                    }
                }
            );
            }
            transactions.Add(new Transaction
            {
                Date = "2020-01-01",
                SizeCode = "S",
                CarrierCode = "MR",
                ShippingPrice = new ShippingPrice
                {
                    PackageSize = "MR",
                    Provider = "S",
                    Price = 2.00,
                    Discount = null
                }
            });
            transactions.Add(new Transaction
            {
                Date = "2020-02-01",
                SizeCode = "S",
                CarrierCode = "MR",
                ShippingPrice = new ShippingPrice
                {
                    PackageSize = "MR",
                    Provider = "S",
                    Price = 2.00,
                    Discount = null
                }
            });

            //Act
            var discountedTransactions = new AssignShippingDiscount().AssignShippingDiscounts(transactions);

            //Assert
            Assert.AreEqual(1.50, discountedTransactions[0].ShippingPrice.Discount.ReducedPrice);
            Assert.AreEqual(0, discountedTransactions[8].ShippingPrice.Discount.ReducedPrice);
            Assert.AreEqual(Math.Round(1.90), Math.Round(discountedTransactions[10].ShippingPrice.Discount.ReducedPrice));
            Assert.AreEqual(1.50, discountedTransactions[11].ShippingPrice.Discount.ReducedPrice);
        }
    }
}
