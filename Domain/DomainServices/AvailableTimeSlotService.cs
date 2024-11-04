using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users.Doctors;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.DomainServices
{
    public class AvailableTimeSlotService
    {
        public static Result<List<TimeSlot>> GetAvailableTimeSlotsForDay(Doctor doctor, MedicalProcedure medicalProcedure, DateOnly selectedDate)
        {
            if (doctor == null)
                return Result.Fail(new FluentResults.Error("Doctor cannot be null."));

            if (medicalProcedure == null)
                return Result.Fail(new FluentResults.Error("MedicalProcedure cannot be null."));

            var availableTimeSlots = new List<TimeSlot>();

            TimeSpan startTime = doctor.WorkingHours.StartTime; 
            TimeSpan endTime = doctor.WorkingHours.EndTime;

            // Retrieve existing appointments for this doctor on the selected day
            var existingAppointments = doctor.GetPlannedAppointments();
                    

            while (startTime.Add(medicalProcedure.Duration) <= endTime)
            {
                var slotEndTime = startTime.Add(medicalProcedure.Duration);
                var timeSlot = TimeSlot.Create(startTime, slotEndTime).Value;

                // Check if the time slot is available by ensuring no overlap with existing appointments
                if (IsTimeAvailable(existingAppointments, timeSlot))
                {
                    availableTimeSlots.Add(timeSlot);
                }

                startTime = slotEndTime; // Move to the next time slot
            }

            return Result.Ok(availableTimeSlots);
        }

        private static bool IsTimeAvailable(IReadOnlyCollection<Appointment> existingAppointments, TimeSlot timeSlot)
        {
            TimeSpan slotStartTime = timeSlot.StartTime;
            TimeSpan slotEndTime = timeSlot.EndTime;

            if (existingAppointments == null || !existingAppointments.Any())
            {
                return true; // If there are no existing appointments, the time slot is available
            }

            foreach (var appointment in existingAppointments)
            {
                // Check if the proposed time slot overlaps with the existing appointment's time slot
                if (timeSlot.OverlapsWith(appointment.Duration))
                {
                    return false; // Overlap found, time is not available
                }
            }

            return true; // No overlaps found, time is available
        }
    }
}