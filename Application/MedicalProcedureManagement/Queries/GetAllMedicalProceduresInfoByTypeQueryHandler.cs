using Application.Configuration.Queries;
using Application.MedicalProcedureManagement.DTO;
using AutoMapper;
using Domain.MedicalProcedures;
using FluentResults;

namespace Application.MedicalProcedureManagement.Queries
{
    public record GetAllMedicalProceduresInfoByTypeQuery(MedicalProcedureType Type)
        : IQuery<Result<ICollection<MedicalProcedureInfoDto>>>;

    public class GetAllMedicalProceduresInfoByTypeQueryHandler
        : IQueryHandler<GetAllMedicalProceduresInfoByTypeQuery, Result<ICollection<MedicalProcedureInfoDto>>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;

        public GetAllMedicalProceduresInfoByTypeQueryHandler(IMedicalProcedureRepository medicalProcedureRepository, 
            IMapper mapper)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<MedicalProcedureInfoDto>>> Handle(GetAllMedicalProceduresInfoByTypeQuery request, 
            CancellationToken cancellationToken)
        {
            var medicalProcedures = await _medicalProcedureRepository.GetAllByTypeAsync(request.Type);

            var medicalProcedureInfoDtos = _mapper.Map<ICollection<MedicalProcedureInfoDto>>(medicalProcedures);

            return Result.Ok(medicalProcedureInfoDtos);

        }
    }
}
