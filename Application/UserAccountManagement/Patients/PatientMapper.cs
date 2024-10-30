using Application.UserAccountManagement.Patients.UserDtos;
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
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.ParseExact(src.DateOfBirth, "dd.MM.yyyy")))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ConstructUsing(src => Patient.Create(
                    new UserParams(src.FirstName, src.LastName, DateOnly.ParseExact(src.DateOfBirth, "dd.MM.yyyy"), src.Email, src.PhoneNumber)).Value);

            CreateMap<Patient, UserFormDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.DateOfBirth,opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Value));
        }
    }
  }
