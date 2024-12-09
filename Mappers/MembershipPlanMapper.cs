using Api.Model.Memberships;

namespace Api.Mappers
{
    public static class MembershipPlanMapper
    {
        public static MembershipPlanResponseDto MapToMembershipPlanResponse(this MembershipPlan request)
        {
            return new MembershipPlanResponseDto(
                    request.Id,
                    request.Name,
                    request.Price,
                    request.Membership.Name,
                    request.RecurringPaymentPlan?.PaymentFrequency,
                    request.RecurringPaymentPlan?.AmtPeriods);
        }
    }
}