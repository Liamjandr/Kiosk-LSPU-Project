using Org.BouncyCastle.Asn1.Cms;
using Paymongo.Sharp;
using Paymongo.Sharp.Core.Enums;
using Paymongo.Sharp.Features.Checkouts.Contracts;
using Paymongo.Sharp.Features.Payments.Contracts;
using Paymongo.Sharp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace kiosk
{
    public class Paymongo
    {
        public string checkoutId;

        // Might be need to use .ENV file or other secure storage for API keys
        PaymongoClient client = new PaymongoClient("sk_test_L7zJE1QknL3NaNJPVqqJgVwr");

        public async Task<string> CreateCheckout(receiptTemplate receipt)
        {
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

        public async Task<bool> CheckPayment(CancellationToken cancelProcess)
        {
            string result = "";
            while (true)
            {
                Checkout getLink = await client.Checkouts.RetrieveCheckoutAsync(checkoutId);
                result = (getLink.Data.Attributes.Payments.Any()) ? getLink.Data.Attributes.Payments.First().Attributes.Status.ToString().ToLower() : "" ;

                if (result == "paid") return true;
                else if (result == "failed") return false;

                //if (getLink.Data.Attributes.Payments != null && getLink.Data.Attributes.Payments.Any())
                //{
                //    var payment = getLink.Data.Attributes.Payments.First();       
                //    return
                //        $"Success " +
                //        $"Paid on {payment.Attributes.PaidAt} " +
                //        $"using {payment.Attributes.Source.Type} " +
                //        $"\nfee: ₱{payment.Attributes.Amount.ToDecimalAmount()} " +
                //        $"\nadditional Service Fee: ₱{payment.Attributes.Fee.ToDecimalAmount()}" +
                //        $"\nTotal: ₱{payment.Attributes.Amount.ToDecimalAmount()+ payment.Attributes.Fee.ToDecimalAmount()} " +
                //        $"\nPayment Status: {payment.Attributes.Status}" 
                //        ;
                //}

                await Task.Delay(500, cancelProcess);
            }
        }

        public async Task<string> CheckExpire()
        {
            Checkout expiredCheckout = await client.Checkouts.ExpireCheckoutAsync(checkoutId);
            return expiredCheckout.Data.Attributes.Status.ToString().ToLower();     
        }
    }
}
