using Domain.SeedWork;

namespace Domain.Appointments.Rules
{
    public class FeedbackCanBeAddedOnlyIfStatusIsCompletedRule : IBusinessRule
    {
        private readonly Appointment _appointment;
        public FeedbackCanBeAddedOnlyIfStatusIsCompletedRule(Appointment appointment)
        {
            _appointment = appointment;
        }
        public bool IsBroken()
        {
            return _appointment.Status != AppointmentStatus.COMPLETED;
        }

        public string Message => "You can add feedback only if appointment status is completed";
    }
}
