using Domain.Doctors;
using Domain.MedicalServices;
using Domain.SeedWork;

namespace Domain.DomainServices
{
    public class AvailableTimeSlotService
    {
        public List<TimeSlot> GetAvailableTimeSlots(Doctor doctor, MedicalProcedure medicalProcedure)
        {
            var availableTimeSlots = new List<TimeSlot>();
            DateTime startTime = doctor.WorkingHours.StartTime.Date;
            DateTime endTime = doctor.WorkingHours.EndTime.Date;

            // Generate possible time slots based on the medical procedure duration
            while (startTime.Add(medicalProcedure.Duration) <= endTime)
            {
                var timeSlot = new TimeSlot(startTime, startTime.Add(medicalProcedure.Duration));

                if (IsTimeAvailable(doctor,timeSlot))
                {
                    availableTimeSlots.Add(timeSlot);
                }
                startTime = startTime.Add(medicalProcedure.Duration); // Move to the next time slot
            }

            return availableTimeSlots;
        }

        private bool IsTimeAvailable(Doctor doctor, TimeSlot timeSlot)
        {
            // Check if the time slot is within working hours
            if (timeSlot.StartTime < doctor.WorkingHours.StartTime || timeSlot.EndTime > doctor.WorkingHours.EndTime)
            {
                return false;
            }

            // Check for overlaps with occupied appointments
            return !doctor.GetPlannedAppointments().Any(a => a.AppointmentDateTime.OverlapsWith(timeSlot));
        }

        //to add the logic for deleting appointment from schedule if it is completed or canceled
        //so in the schedule are stored only planned appointments
    }
}
