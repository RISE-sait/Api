using Api.enums;

namespace Api.Interfaces
{
    public interface ICoach
    {
        void AddCustomerToTeam(Guid customerId, Guid teamId)
        {
            throw new NotImplementedException();
        }

        void RemoveCustomerFromTeam(Guid customerId, Guid teamId) {
            throw new NotImplementedException();
        }
        void AmendCourseSchedule(Guid courseId, ScheduleDayTime[] newSchedule) {
            throw new NotImplementedException();
        }

        void ManageBooking(Guid facilityId, DateTime startDateTime, DateTime endDateTime) {

        }
    }

    public struct ScheduleDayTime
    {
        public DaysInWeekEnum Day;
        public TimeOnly StartTime;
        public TimeOnly EndTime;
    }
}