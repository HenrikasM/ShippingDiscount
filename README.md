# ShippingDiscount
Start:
1)Open ShippingDiscount .exe file in "ShippingDiscount\ShippingDiscount\bin\Debug\netcoreapp2.1\publish" .
2)Type "input.txt" for default run(Files are taken from DataImports/DataFiles/).
3)Type "Y" to run tests.

Domains:
Main domain for information from file.
Transaction
        public string Date { get; set; }
        public string SizeCode { get; set; }
        public string CarrierCode { get; set; }
        public ShippingPrice ShippingPrice { get; set; }
        
Domain for base price information.
ShippingPrice
        public string Provider { get; set; }
        public string PackageSize { get; set; }
        public double Price { get; set; }
        public ShippingPriceDiscount Discount { get; set; }
        
Domain for Discounted price information.
ShippingPriceDiscount
        public double ReducedPrice { get; set; }
        public double DiscountedAmount { get; set; }


Test logic
TestCase1 - Check if data after discount from input.txt is the same as from correctOutput.txt
TestCase2 - Check if transaction + shipping price(without discount) is created as expected from input(2020-01-01 S LP)
expected data - Date = "2020-01-01",
                SizeCode = "S",
                CarrierCode = "LP",
                ShippingPrice = new ShippingPrice
                {
                    PackageSize = "S",
                    Provider = "LP",
                    Price = 1.50,
                    Discount = null
                }
TestCase3 - Discount rule 1. Check if input "2020-01-01 S MR" returns transaction with discount.
TestCase4 - Discount rule 2. Check if 3rd "L"&&"LP" transaction is free.
TestCase5 - Discount rule 3. Check if discount amount is not bigger than 10EU(month).
