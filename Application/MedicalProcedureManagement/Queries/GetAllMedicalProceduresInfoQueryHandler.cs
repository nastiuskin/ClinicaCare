using Application.Configuration.Queries;
using Application.MedicalProcedureManagement.DTO;
using AutoMapper;
using Domain.MedicalProcedures;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.MedicalProcedureManagement.Queries
{
    public record GetAllMedicalProceduresInfoQuery(int PageNumber, int PageSize)
        : IQuery<Result<ICollection<MedicalProcedureInfoDto>>>;


    public class GetAllMedicalProceduresInfoQueryHandler
        : IQueryHandler<GetAllMedicalProceduresInfoQuery, Result<ICollection<MedicalProcedureInfoDto>>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;

        public GetAllMedicalProceduresInfoQueryHandler(
            IMedicalProcedureRepository medicalProcedureRepository,
            IMapper mapper)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<MedicalProcedureInfoDto>>> Handle(GetAllMedicalProceduresInfoQuery query,
            CancellationToken cancellationToken)
        {
            var medicalProcedures = await _medicalProcedureRepository.GetAllAsync(query.PageNumber, query.PageSize).ToListAsync();

            var medicalProcedureInfoDtos = _mapper.Map<ICollection<MedicalProcedureInfoDto>>(medicalProcedures);

            return Result.Ok(medicalProcedureInfoDtos);
        }
    }
}
