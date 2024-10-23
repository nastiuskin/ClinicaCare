using Application.MedicalProcedureManagement.Queries.QueryObjects;
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
        }
    }
}
