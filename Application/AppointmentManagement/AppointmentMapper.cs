using Application.AppointmentManagement.DTO;
using AutoMapper;
using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.ValueObjects;

namespace Application.AppointmentManagement
{
    public class AppointmentMapper : Profile
    {
        public AppointmentMapper() 
        {
            CreateMap<AppointmentCreateDto, Appointment>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Date, "dd.MM.yyyy")))
                .ConstructUsing(src => Appointment.Create(new UserId(src.DoctorId), new UserId(src.PatientId), new MedicalProcedureId(src.MedicalProcedureId),
                    DateOnly.ParseExact(src.Date, "dd.MM.yyyy"), TimeSlot.Create(TimeSpan.Parse(src.StartTime), TimeSpan.Parse(src.EndTime)).Value).Value);
        }
    }
}
