using Domain.MedicalProcedures;
using Domain.Users.Doctors;
using Domain.Users.Patients;
using Domain.ValueObjects;

namespace Domain.Appointments
{
    public record AppointmentParams(Doctor Doctor, Patient Patient, MedicalProcedure MedicalProcedure, TimeSlot AppointmentDateTime);
}
