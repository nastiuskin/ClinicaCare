using Application.Configuration.Queries;
using Application.UserAccountManagement.Doctors.DTO;
using Application.UserAccountManagement.Patients.UserDtos;
using AutoMapper;
using Domain.Users;
using FluentResults;

namespace Application.UserAccountManagement.Patients.Queries
{
    public record GetAllPatientsInfoQuery
        : IQuery<Result<ICollection<UserFormDto>>>;

    public class GetAllPatientsInfoQueryHandler
        : IQueryHandler<GetAllPatientsInfoQuery, Result<ICollection<UserFormDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllPatientsInfoQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<Result<ICollection<UserFormDto>>> Handle(GetAllPatientsInfoQuery request, CancellationToken cancellationToken)
        {
            var patients = await _userRepository.GetAllPatientsAsync();

            var patientsInfoDtos = _mapper.Map<ICollection<UserFormDto>>(patients);

            return Result.Ok(patientsInfoDtos);
        }
    }
}
