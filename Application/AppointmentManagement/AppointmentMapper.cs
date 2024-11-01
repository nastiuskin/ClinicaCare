using Application.AppointmentManagement.DTO;
using AutoMapper;
using Domain.Appointments;

namespace Application.AppointmentManagement
{
    public class AppointmentMapper : Profile
    {
        public AppointmentMapper() 
        {
            CreateMap<AppointmentCreateDto, Appointment>()
                .ConstructUsing(src => Appointment.Create(src.DoctorId, src.PatientId, src.MedicalProcedureId,
                src.Date, src.Duration).Value);
        }
    }
}
