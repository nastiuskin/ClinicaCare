using Application.MedicalProcedureManagement.DTO;
using AutoMapper;
using Domain.MedicalProcedures;

namespace Application.MedicalProcedureManagement
{
    public class MedicalProcedureMapper : Profile
    {
        public MedicalProcedureMapper()
        {
            CreateMap<MedicalProcedure, MedicalProcedureInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<MedicalProcedureFormDto, MedicalProcedure>()
                .ForMember(dest => dest.Doctors, opt => opt.Ignore())
                .ConstructUsing(src => MedicalProcedure.Create(src.Type, src.Price, TimeSpan.Parse(src.Duration + ":00"), src.Name).Value);

            CreateMap<MedicalProcedureUpdateDto, MedicalProcedure>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<MedicalProcedure, MedicalProcedureInfoWithDoctorsDto>()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
               .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString()))
               .ForMember(dest => dest.Doctors, opt => opt.MapFrom(src => src.Doctors));
        }
    }
}
