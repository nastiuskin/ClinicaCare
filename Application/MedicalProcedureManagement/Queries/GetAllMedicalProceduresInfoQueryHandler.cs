using Application.Configuration.Queries;
using Application.Helpers.PaginationStuff;
using Application.MedicalProcedureManagement.DTO;
using AutoMapper;
using Domain.MedicalProcedures;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.MedicalProcedureManagement.Queries
{
    public record GetAllMedicalProceduresInfoQuery(MedicalProcedureParameters Parameters)
        : IQuery<Result<ICollection<MedicalProcedureInfoDto>>>;


    public class GetAllMedicalProceduresInfoQueryHandler
        : IQueryHandler<GetAllMedicalProceduresInfoQuery, Result<ICollection<MedicalProcedureInfoDto>>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllMedicalProceduresInfoQueryHandler(
            IMedicalProcedureRepository medicalProcedureRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<ICollection<MedicalProcedureInfoDto>>> Handle(GetAllMedicalProceduresInfoQuery query,
            CancellationToken cancellationToken)
        {
            var medicalProcedures = _medicalProcedureRepository.GetMedicalProceduresAsync(query.Parameters);

            var medicalProcedureInfoDtos = _mapper.Map<ICollection<MedicalProcedureInfoDto>>(medicalProcedures);
            _httpContextAccessor.HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(medicalProcedures.MetaData));

            return Result.Ok(medicalProcedureInfoDtos);
        }
    }
}
