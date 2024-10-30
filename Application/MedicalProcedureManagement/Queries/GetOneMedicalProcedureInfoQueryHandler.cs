using Application.Configuration.Queries;
using Application.MedicalProcedureManagement.DTO;
using Application.SeedOfWork;
using AutoMapper;
using Domain.MedicalProcedures;
using FluentResults;

namespace Application.MedicalProcedureManagement.Queries
{
    public record GetOneMedicalProcedureInfoQuery(Guid Id)
        : IQuery<Result<MedicalProcedureInfoWithDoctorsDto>>;


    public class OneMedicalProcedureInfoQueryHandler
        : IQueryHandler<GetOneMedicalProcedureInfoQuery,
        Result<MedicalProcedureInfoWithDoctorsDto>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;

        public OneMedicalProcedureInfoQueryHandler(IMedicalProcedureRepository medicalProcedureRepository,
            IMapper mapper)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
        }

        public async Task<Result<MedicalProcedureInfoWithDoctorsDto>> Handle(GetOneMedicalProcedureInfoQuery query,
            CancellationToken cancellation)
        {
            var medicalProcedure = await _medicalProcedureRepository.GetByIdWithDoctorsAsync(new MedicalProcedureId(query.Id));
            if (medicalProcedure == null)
                return Result.Fail(ResponseError.NotFound(nameof(medicalProcedure), query.Id));

            var medicalProcedureDto = _mapper.Map<MedicalProcedureInfoWithDoctorsDto>(medicalProcedure);

            return Result.Ok(medicalProcedureDto);
        }

    }


}
