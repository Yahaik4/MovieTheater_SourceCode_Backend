using Microsoft.AspNetCore.Mvc;
using PaymentService.ServiceConnector.CinemaService;
using Stripe;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly CinemaServiceConnector _cinemaServiceConnector;

        public StripeWebhookController(CinemaServiceConnector cinemaServiceConnector)
        {
            _cinemaServiceConnector = cinemaServiceConnector;
        }

        [HttpPost]
        public async Task<IActionResult> Receive()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    "YOUR_STRIPE_WEBHOOK_SECRET"
                );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Gọi internal gRPC logic để cập nhật booking/payment
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Gọi internal gRPC logic để xử lý thất bại
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
