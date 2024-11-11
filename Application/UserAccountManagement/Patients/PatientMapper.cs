using Application.UserAccountManagement.UserDtos;
using AutoMapper;
using Domain.Users;
using Domain.Users.Patients;


namespace Application.UserAccountManagement.Patients
{
    public class PatientMapper : Profile
    {
        public PatientMapper()
        {
            CreateMap<UserFormDto, Patient>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.ParseExact(src.DateOfBirth, "dd.MM.yyyy")))
            .ConstructUsing(src => Patient.Create(
                new UserParams(src.FirstName, src.LastName, DateOnly.ParseExact(src.DateOfBirth, "dd.MM.yyyy"), src.Email, src.PhoneNumber)).Value);

            CreateMap<Patient, UserViewDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                             src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToString("dd.MM.yyyy") : null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
  }
