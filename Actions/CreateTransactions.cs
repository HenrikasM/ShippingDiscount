using ShippingDiscount.Actions.PriceActions;
using ShippingDiscount.Domains;

namespace ShippingDiscount.Actions
{
    public class CreateTransactions
    {
        //Create transaction
        public Transaction CreateTransaction(string Date, string SizeCode, string CarrierCode)
        {
            return new Transaction
            {
                Date = Date,
                SizeCode = SizeCode,
                CarrierCode = CarrierCode,
                ShippingPrice = new AssignPrice().AssignShippingPrice(CarrierCode, SizeCode)
            };
        }
    }
}
