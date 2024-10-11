using Domain.Entities.Appointments;
using Domain.Entities.MedicalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Doctors
{
    public class Schedule
    {
        public Doctor Doctor { get; private set; }
        public TimeSpan WorkingHoursStart { get; private set; } //Time Slot - working hours
        public TimeSpan WorkingHoursEnd { get; private set; }

        public TimeSlot WorkingHours {  get; private set; }
        public List<Appointment.Appointment> OccupiedAppointments { get; private set; } //planned appoinmtents or occupied time spans

        public Schedule(Doctor doctor, TimeSpan workingHoursStart, TimeSpan workingHoursEnd)
        {
            Doctor = doctor;
            WorkingHoursStart = workingHoursStart;
            WorkingHoursEnd = workingHoursEnd;
            OccupiedAppointments = new List<Appointment.Appointment>();
        }

        public List<TimeSlot> GetAvailableTimeSlots(MedicalService medicalService)
        {
            var availableSlots = new List<TimeSlot>();
            DateTime startTime = DateTime.Today.Add(WorkingHoursStart); 
            DateTime endTime = DateTime.Today.Add(WorkingHoursEnd);

            // Generate possible time slots based on the service duration
            while (startTime.Add(medicalService.Duration) <= endTime)
            {
                // Use the constructor to create a new TimeSlot
                var timeSlot = new TimeSlot(startTime, startTime.Add(medicalService.Duration));

                if (IsTimeAvailable(timeSlot))
                {
                    availableSlots.Add(timeSlot);
                }
                startTime = startTime.Add(medicalService.Duration); // Move to the next time slot
            }

            return availableSlots;
        }

        public bool IsTimeAvailable(TimeSlot timeSlot)
        {
            // Check if the time slot is within working hours
            if (timeSlot.startTime.TimeOfDay < WorkingHoursStart || timeSlot.endTime.TimeOfDay > WorkingHoursEnd)
            {
                return false;
            }

            // Check for overlaps with occupied appointments
            return !OccupiedAppointments.Any(a => a.AppointmentDateTime.OverlapsWith(timeSlot));
        }

        public void AddAppointment(Appointment.Appointment appointment)
        {
            if (!IsTimeAvailable(appointment.AppointmentDateTime))
            {
                throw new InvalidOperationException("The selected time slot is not available.");
            }
            OccupiedAppointments.Add(appointment);
        }
    }
}
