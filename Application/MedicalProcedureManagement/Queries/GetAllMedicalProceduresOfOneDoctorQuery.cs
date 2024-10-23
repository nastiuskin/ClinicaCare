using Application.Configuration.Queries;
using Application.MedicalProcedureManagement.Queries.QueryObjects;
using AutoMapper;
using Domain.MedicalProcedures;
using Domain.Users;
using FluentResults;

namespace Application.MedicalProcedureManagement.Queries
{
    public record GetAllMedicalProceduresOfOneDoctorQuery(Guid DoctorId) : IQuery<Result<ICollection<MedicalProcedureInfoDto>>>;

    public class GetAllMedicalProceduresOfOneDoctorQueryHandler : IQueryHandler<GetAllMedicalProceduresOfOneDoctorQuery, Result<ICollection<MedicalProcedureInfoDto>>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;

        public GetAllMedicalProceduresOfOneDoctorQueryHandler(IMedicalProcedureRepository medicalProcedureRepository, IMapper mapper)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<MedicalProcedureInfoDto>>> Handle(GetAllMedicalProceduresOfOneDoctorQuery query, CancellationToken cancellation)
        {
            var medicalProcedures = await _medicalProcedureRepository.GetAllByDoctorIdAsync(new UserId(query.DoctorId));

            if (medicalProcedures == null || !medicalProcedures.Any()) return Result.Fail("No medical procedures found.");

            var medicalProcedureDtos = _mapper.Map<ICollection<MedicalProcedureInfoDto>>(medicalProcedures);

            return Result.Ok(medicalProcedureDtos);

        }
    }
}
