using System.Threading.Tasks;

namespace Azunt.Web.Payments.Stripe
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(
            decimal amount,
            string currency,
            string successUrl,
            string cancelUrl);

        Task<object?> HandleWebhookAsync(string json, string stripeSignatureHeader);
    }
}