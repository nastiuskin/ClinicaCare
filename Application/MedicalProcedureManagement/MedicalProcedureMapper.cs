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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

            CreateMap<MedicalProcedureFormDto, MedicalProcedure>()
                .ConstructUsing(src => MedicalProcedure.Create(src.Type, src.Price, TimeSpan.Parse(src.Duration + ":00"), src.Name).Value);
        }
    }
}
