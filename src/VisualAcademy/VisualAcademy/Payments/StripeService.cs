using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace Azunt.Web.Payments.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly StripeOptions _options;

        public StripeService(IOptions<StripeOptions> options)
        {
            _options = options.Value;
            StripeConfiguration.ApiKey = _options.SecretKey;
        }

        public async Task<string> CreateCheckoutSessionAsync(
            decimal amount,
            string currency,
            string successUrl,
            string cancelUrl)
        {
            var service = new SessionService();

            var sessionOptions = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = amount * 100, // Stripe는 센트 단위 사용
                            Currency = currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Sample Product"
                            }
                        }
                    }
                }
            };

            var session = await service.CreateAsync(sessionOptions);
            return session.Url; // Redirect URL 반환
        }

        public Task<object?> HandleWebhookAsync(string json, string stripeSignatureHeader)
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignatureHeader,
                _options.WebhookSecret
            );

            // TODO. stripeEvent.Type에 따라 비즈니스 로직 처리

            return Task.FromResult<object?>(stripeEvent);
        }
    }
}