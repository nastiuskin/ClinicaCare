using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.Patients;
using Domain.ValueObjects;

namespace Domain.Appointments
{
    public record AppointmentParams(Doctor Doctor, Patient Patient, MedicalProcedure MedicalProcedure, TimeSlot AppointmentDateTime);
}
