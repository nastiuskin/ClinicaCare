using Domain.MedicalProcedures;
using Domain.Users.Doctors;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.DomainServices
{
    public class AvailableTimeSlotService
    {
        public Result<List<TimeSlot>> GetAvailableTimeSlots(Doctor doctor, MedicalProcedure medicalProcedure)
        {
            if (doctor == null)
                return Result.Fail(new FluentResults.Error("Doctor cannot be null."));

            if (medicalProcedure == null)
                return Result.Fail(new FluentResults.Error("MedicalProcedure cannot be null."));

            if (doctor.WorkingHours == null)
                return Result.Fail(new FluentResults.Error("Doctor's working hours cannot be null."));

            var availableTimeSlots = new List<TimeSlot>();
            DateTime startTime = doctor.WorkingHours.StartTime.Date;
            DateTime endTime = doctor.WorkingHours.EndTime.Date;

            // Generate possible time slots based on the medical procedure duration
            while (startTime.Add(medicalProcedure.Duration) <= endTime)
            {
                var timeSlot = TimeSlot.Create(startTime, startTime.Add(medicalProcedure.Duration)).Value;

                if (IsTimeAvailable(doctor, timeSlot))
                {
                    availableTimeSlots.Add(timeSlot);
                }
                startTime = startTime.Add(medicalProcedure.Duration); // Move to the next time slot
            }

            return Result.Ok(availableTimeSlots);
        }

        private bool IsTimeAvailable(Doctor doctor, TimeSlot timeSlot)
        {
            if (timeSlot.StartTime < doctor.WorkingHours.StartTime || timeSlot.EndTime > doctor.WorkingHours.EndTime)
                return false;

            return !doctor.GetPlannedAppointments().Any(a => a.AppointmentDateTime.OverlapsWith(timeSlot));
        }
    }
}
