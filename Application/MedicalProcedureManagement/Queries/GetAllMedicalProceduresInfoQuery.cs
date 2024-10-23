﻿using Application.Configuration.Queries;
using Application.MedicalProcedureManagement.Queries.QueryObjects;
using AutoMapper;
using Domain.MedicalProcedures;
using FluentResults;

namespace Application.MedicalProcedureManagement.Queries
{
    public record GetAllMedicalProceduresInfoQuery : IQuery<Result<ICollection<MedicalProcedureInfoDto>>>;


    public class GetAllMedicalProceduresInfoQueryHandler : IQueryHandler<GetAllMedicalProceduresInfoQuery, Result<ICollection<MedicalProcedureInfoDto>>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;

        public GetAllMedicalProceduresInfoQueryHandler(IMedicalProcedureRepository medicalProcedureRepository, IMapper mapper)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<MedicalProcedureInfoDto>>> Handle(GetAllMedicalProceduresInfoQuery query, CancellationToken cancellationToken)
        {
            var medicalProcedures = await _medicalProcedureRepository.GetAllAsync();

            if (medicalProcedures == null || !medicalProcedures.Any()) return Result.Fail("No medical procedures found.");

            var medicalProcedureInfoDtos = _mapper.Map<ICollection<MedicalProcedureInfoDto>>(medicalProcedures);

            return Result.Ok(medicalProcedureInfoDtos);
        }
    }
}
