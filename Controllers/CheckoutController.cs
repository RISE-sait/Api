using Api.Database;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController(AppDbContext dbContext) : ControllerBase
    {

        [HttpPost("membership")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateMembershipCheckoutSessionRequest request)
        {
            var membership = dbContext.Memberships.FirstOrDefault(m => m.Name == request.MembershipName);
            
            if (membership == default) 
                return NotFound(new { error = "Membership not found" });

            var price = membership.Price;

            var domain = "http://localhost:5000";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes =
                [
                    "card",
                ],
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = price,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = request.MembershipName,
                            },
                        },
                        Quantity = 1,
                    },
                ],
                Mode = "payment",
                SuccessUrl = $"{domain}/success.html?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/cancel.html",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Ok(session.Url);
        }
    }

    public readonly record struct CreateMembershipCheckoutSessionRequest(string MembershipName);
}