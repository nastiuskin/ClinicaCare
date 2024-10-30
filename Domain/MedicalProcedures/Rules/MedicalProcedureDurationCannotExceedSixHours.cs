using Domain.SeedWork;

namespace Domain.MedicalProcedures.Rules
{
    public class MedicalProcedureDurationCannotExceedSixHours : IBusinessRule
    {
        private readonly TimeSpan _duration;
        public MedicalProcedureDurationCannotExceedSixHours(TimeSpan duration)
        {
            _duration = duration;
        }

        public bool IsBroken()
        {
            return _duration > TimeSpan.FromHours(6);
        }

        public string Message => "Medical Procedure duration cannot exceed 6 hours";
    }
}
