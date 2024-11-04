using Application.UserAccountManagement.Doctors.DTO;
using AutoMapper;
using Domain.Users;
using Domain.Users.Doctors;
using Domain.ValueObjects;

namespace Application.UserAccountManagement.Doctors
{
    public class DoctorMapper : Profile
    {
        public DoctorMapper()
        {
            CreateMap<DoctorFormDto, Doctor>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.ParseExact(src.DateOfBirth, "dd.MM.yyyy")))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.ToString()))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
                .ForMember(dest => dest.CabinetNumber, opt => opt.MapFrom(src => src.CabinetNumber))
                .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(
                    src => TimeSlot.Create(TimeSpan.Parse(src.WorkingHoursStart + ":00"), TimeSpan.Parse(src.WorkingHoursEnd + ":00")).Value))
                .ConstructUsing(src => Doctor.Create(
                    new UserParams(src.FirstName, src.LastName, DateOnly.ParseExact(src.DateOfBirth, "dd.MM.yyyy"), src.Email, src.PhoneNumber),
                    new DoctorParams(src.Specialization, src.Biography, src.CabinetNumber, TimeSpan.Parse(src.WorkingHoursStart + ":00"), TimeSpan.Parse(src.WorkingHoursEnd + ":00"))
                ).Value);


            CreateMap<Doctor, DoctorPartialInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.SpecializationType, opt => opt.MapFrom(src => src.Specialization));
        }
    }
}