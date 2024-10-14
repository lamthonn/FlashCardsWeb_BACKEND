using backend_v3.Dto;
using backend_v3.Models;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace backend_v3.Services
{
    public class StripePaymentService
    {
        public StripePaymentService(IOptions<StripeSettings> stripeSettings)
        {
            StripeConfiguration.ApiKey = stripeSettings.Value.SecretKey;
        }

        public Session CreateCheckoutSession(PaymentParam param)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>{"card"},
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = param.priceInCents, // For example, $20.00 (this value is in cents) =>  Giá được truyền động từ phía client
                        Currency = "vnd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = param.productName,
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = param.successUrl,
                CancelUrl = param.cancelUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }
    }
}
