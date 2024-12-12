using Api.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var membership = dbContext.Memberships
            .Include(m => m.MembershipPlans)
            .FirstOrDefault(m => m.Name == request.MembershipName);

            if (membership == default)
                return NotFound(new ProblemDetails
                {
                    Title = "Membership Not Found",
                    Detail = "The specified membership could not be found.",
                    Status = StatusCodes.Status404NotFound
                });
            var membershipPlan = membership.MembershipPlans.FirstOrDefault(mp => mp.Name == request.PlanName);
            if (membershipPlan == default)
                return NotFound(new ProblemDetails
                {
                    Title = "Membership Plan Not Found",
                    Detail = "The specified membership plan could not be found.",
                    Status = StatusCodes.Status404NotFound
                });

            var price = membershipPlan.Price;

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
                             Recurring = membershipPlan.RecurringPaymentPlan != null ? new SessionLineItemPriceDataRecurringOptions
                        {
                            Interval = membershipPlan.RecurringPaymentPlan.PaymentFrequency.ToString().ToLower(),
                            IntervalCount = membershipPlan.RecurringPaymentPlan.AmtPeriods ?? null
                        } : null
                        },
                        Quantity = 1,
                    },
                ],
                Mode = membershipPlan.RecurringPaymentPlan != null ? "subscription" : "payment",
                SuccessUrl = $"{domain}/success.html?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/cancel.html",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(session.Url);
        }
    }

    public readonly record struct CreateMembershipCheckoutSessionRequest(string MembershipName, string PlanName);
}