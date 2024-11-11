using Api.Database;
using Api.helpers;

namespace Api.Interfaces
{
    public interface ICoach
    {
        void AddCustomerToTeam(Guid customerId, Guid teamId)
        {
            throw new NotImplementedException();
        }

        void RemoveCustomerFromTeam(Guid customerId, Guid teamId)
        {
            throw new NotImplementedException();
        }

        async Task ManageBooking(AppDbContext context, BookingInfo bookingInfo)
        {
            if (await ScheduleHelper.IsBookingOverlapping(context, bookingInfo))
            {
                throw new InvalidOperationException("The course schedule overlaps with an existing schedule.");
            }
        }
    }

    public struct BookingInfo
    {
        public Guid FacilityId;
        public DateTime StartDateTime;
        public DateTime EndDateTime;
    }
}