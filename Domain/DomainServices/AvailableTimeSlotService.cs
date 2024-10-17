using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.SeedWork.ValueObjects;

namespace Domain.DomainServices
{
    public class AvailableTimeSlotService
    {
        public List<TimeSlot> GetAvailableTimeSlots(Doctor doctor, MedicalProcedure medicalProcedure)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            if (medicalProcedure == null)
            {
                throw new ArgumentNullException(nameof(medicalProcedure), "MedicalProcedure cannot be null.");
            }

            // Check if doctor's working hours are set
            if (doctor.WorkingHours == null)
            {
                throw new InvalidOperationException("Doctor's working hours cannot be null.");
            }

            var availableTimeSlots = new List<TimeSlot>();
            DateTime startTime = doctor.WorkingHours.StartTime.Date;
            DateTime endTime = doctor.WorkingHours.EndTime.Date;

            // Generate possible time slots based on the medical procedure duration
            while (startTime.Add(medicalProcedure.Duration) <= endTime)
            {
                var timeSlot = new TimeSlot(startTime, startTime.Add(medicalProcedure.Duration));

                if (IsTimeAvailable(doctor, timeSlot))
                {
                    availableTimeSlots.Add(timeSlot);
                }
                startTime = startTime.Add(medicalProcedure.Duration); // Move to the next time slot
            }

            return availableTimeSlots;
        }

        private bool IsTimeAvailable(Doctor doctor, TimeSlot timeSlot)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            if (timeSlot == null)
            {
                throw new ArgumentNullException(nameof(timeSlot), "TimeSlot cannot be null.");
            }
            // Check if the time slot is within working hours
            if (timeSlot.StartTime < doctor.WorkingHours.StartTime || timeSlot.EndTime > doctor.WorkingHours.EndTime)
            {
                return false;
            }

            // Check for overlaps with occupied appointments
            return !doctor.GetPlannedAppointments().Any(a => a.AppointmentDateTime.OverlapsWith(timeSlot));
        }
    }
}
