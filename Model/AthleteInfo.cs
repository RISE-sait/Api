using System.ComponentModel.DataAnnotations;
using Api.Model.People.Customers;

namespace Api.Model
{
    public class BasicAthleteInfo(Guid customerId)
    {
        public Customer Customer { get; set; } = null!;
        [Key] public Guid CustomerId { get; set; } = customerId;

        public int? Age { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public DateOnly? Dob { get; set; }
        public char? Gender { get; set; }
        private int? Attendance { get; set; }
    }

    public class AdvancedAthleteInfo(Guid customerId)
    {
        public Customer Customer { get; set; } = null!;
        [Key] public Guid CustomerId { get; set; } = customerId;

        private double? Ppg { get; set; }
        private double? Rpg { get; set; }
    }
}