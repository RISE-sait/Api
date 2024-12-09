using System.ComponentModel.DataAnnotations;

namespace Api.Model.Memberships
{
    public sealed record CreateMembershipPlanDto(
        [StringLength(50, MinimumLength = 1)] string Name,
        long Price,
        Guid MembershipId,
        string? PaymentFrequencyStr = null,
        [Range(1, 100)] int? AmtPeriods = null) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var hasFrequency = !string.IsNullOrEmpty(PaymentFrequencyStr);
            var hasPeriods = AmtPeriods.HasValue;

            if (hasFrequency != hasPeriods)
            {
                yield return new ValidationResult(
                    "PaymentFrequency and AmtPeriods must both be specified or both be null",
                    [nameof(PaymentFrequency), nameof(AmtPeriods)]
                );
            }

            if (hasFrequency && !Enum.TryParse<PaymentFrequency>(PaymentFrequencyStr, true, out _))
            {
                yield return new ValidationResult(
                    $"Invalid payment frequency. Valid values are: {string.Join(", ", Enum.GetNames<PaymentFrequency>())}",
                    [nameof(PaymentFrequencyStr)]
                );
            }

            if (Price <= 0)
            {
                yield return new ValidationResult(
                    "Price must be greater than 0",
                    [nameof(Price)]
                );
            }

            if (hasPeriods && AmtPeriods <= 0)
            {
                yield return new ValidationResult(
                    "Number of payment periods must be greater than 0",
                    [nameof(AmtPeriods)]
                );
            }
        }

        public MembershipPlan ToMembershipPlan() => new(Name, Price, MembershipId)
        {
            RecurringPaymentPlan = !string.IsNullOrEmpty(PaymentFrequencyStr)
                ? new RecurringPaymentPlan
                {
                    PaymentFrequency = Enum.Parse<PaymentFrequency>(PaymentFrequencyStr, true),
                    AmtPeriods = AmtPeriods
                }
                : null
        };
    }

    public readonly record struct MembershipPlanResponseDto(
           Guid Id,
           string Name,
           long Price,
           string MembershipName,
            PaymentFrequency? PaymentFrequency = null,
            int? AmtPeriods = null
       );
}