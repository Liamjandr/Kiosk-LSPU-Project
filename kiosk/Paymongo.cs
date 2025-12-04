using Org.BouncyCastle.Asn1.Cms;
using Paymongo.Sharp;
using Paymongo.Sharp.Core.Enums;
using Paymongo.Sharp.Features.Checkouts.Contracts;
using Paymongo.Sharp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace kiosk
{
    public class Paymongo
    {
        public string checkoutId;

        public async Task<string> CreateCheckout(receiptTemplate receipt)
        {
            var client = new PaymongoClient("sk_test_L7zJE1QknL3NaNJPVqqJgVwr");

            Checkout checkout = new Checkout()
            {
                Data = new CheckoutData()
                {
                    Attributes = new CheckoutAttributes()
                    {
                        Description = "Test Checkout",
       

                        LineItems = receipt.Items.Select(item => new LineItem
                        {
                            Name = item.Name,
                            Quantity = item.Quantity,
                            Currency = Currency.Php,
                            Amount = Convert.ToInt64(item.Price) * 100
                        }).ToList()
                        
                        //LineItems = new[] {


                        //    new LineItem {
                        //        Name = "item_name",
                        //        Quantity = 1,
                        //        Currency = Currency.Php,
                        //        Amount = 3500
                        //    }
                        //}
                        ,
                        PaymentMethodTypes = new[] {
                    PaymentMethodType.GCash,
                    PaymentMethodType.Card,
                    PaymentMethodType.Paymaya
                }
                    }
                }
            };
            
            Checkout checkoutResult = await client.Checkouts.CreateCheckoutAsync(checkout);
            
            // Save ID so the second function can poll it
            this.checkoutId = checkoutResult.Data.Id;
            

            return checkoutResult.Data.Attributes.CheckoutUrl;
        }

        public async Task<string> CheckPayment()
        {
            var client = new PaymongoClient("sk_test_L7zJE1QknL3NaNJPVqqJgVwr");

            while (true)
            {
                var getLink = await client.Checkouts.RetrieveCheckoutAsync(checkoutId);
                //getLink.Data.Attributes.Payments
                if (getLink.Data.Attributes.Payments != null && getLink.Data.Attributes.Payments.Any())
                {
                    var payment = getLink.Data.Attributes.Payments.First();

                    return
                        $"Paid on {payment.Attributes.PaidAt} " +
                        $"using {payment.Attributes.Source.Type} " +
                        $"\nfee: ₱{payment.Attributes.Amount.ToDecimalAmount()} " +
                        $"\nadditional Service Fee: ₱{payment.Attributes.Fee.ToDecimalAmount()}" +
                        $"\nTotal: ₱{payment.Attributes.Amount.ToDecimalAmount()+ payment.Attributes.Fee.ToDecimalAmount()} " +
                         $"\n{payment.Attributes.Status}";
                }

                await Task.Delay(500);
            }
        }
    }
}
