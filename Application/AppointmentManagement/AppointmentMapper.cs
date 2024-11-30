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
            CreateMap<TimeSlotDto, TimeSlot>()
                 .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
                 .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)));


            CreateMap<AppointmentFormDto, Appointment>()
              .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Date, "dd/MM/yyyy")))
              .ConstructUsing(src => Appointment.Create(
                  new UserId(src.DoctorId),
                  new UserId(src.PatientId),
                  new MedicalProcedureId(src.MedicalProcedureId),
                  DateOnly.ParseExact(src.Date, "dd/MM/yyyy"),  
                  TimeSlot.Create(TimeSpan.Parse(src.Duration.StartTime), TimeSpan.Parse(src.Duration.EndTime)).Value
              ).Value);



            CreateMap<Appointment, AppointmentInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
                .ForMember(dest => dest.MedicalProcedureName, opt => opt.MapFrom(src => src.MedicalProcedure.Name))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Duration.StartTime.ToString("hh\\:mm")))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Duration.EndTime.ToString("hh\\:mm")))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.FeedBack, opt => opt.MapFrom(src => src.DoctorFeedback));
        }
    }
}
