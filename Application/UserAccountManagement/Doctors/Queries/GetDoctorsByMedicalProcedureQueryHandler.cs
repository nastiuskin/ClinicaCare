using Application.Configuration.Queries;
using Application.MedicalProcedureManagement.DTO;
using Application.UserAccountManagement.Doctors.DTO;
using AutoMapper;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccountManagement.Doctors.Queries
{
    public record GetDoctorsByMedicalProcedureQuery(Guid MedicalProcedureId)
       : IQuery<Result<ICollection<DoctorPartialInfoDto>>>;

    public class GetDoctorsByMedicalProcedureQueryHandler
        : IQueryHandler<GetDoctorsByMedicalProcedureQuery, Result<ICollection<DoctorPartialInfoDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetDoctorsByMedicalProcedureQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<DoctorPartialInfoDto>>> Handle(GetDoctorsByMedicalProcedureQuery request,
            CancellationToken cancellationToken)
        {
            var query = _userRepository.GetAll();
            var doctors = query.OfType<Doctor>().Where(d => d.MedicalProcedures.Any(mp => mp.Id.Equals(request.MedicalProcedureId))).ToList();

            var doctorDtos = _mapper.Map<ICollection<DoctorPartialInfoDto>>(doctors);

            return Result.Ok(doctorDtos);

        }
    }
}


