using Application.Configuration.Queries;
using Application.UserAccountManagement.Doctors.DTO;
using Application.UserAccountManagement.UserDtos;
using AutoMapper;
using Domain.Helpers.PaginationStuff;
using Domain.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.UserAccountManagement.Patients.Queries
{
    public record GetAllPatientsInfoQuery(UserParameters Parameters)
        : IQuery<Result<ICollection<UserViewDto>>>;

    public class GetAllPatientsInfoQueryHandler
        : IQueryHandler<GetAllPatientsInfoQuery, Result<ICollection<UserViewDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllPatientsInfoQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<Result<ICollection<UserViewDto>>> Handle(GetAllPatientsInfoQuery request, CancellationToken cancellationToken)
        {
            var patients = _userRepository.GetAllPatientsAsync(request.Parameters);

            var patientsInfoDtos = _mapper.Map<ICollection<UserViewDto>>(patients);

            return Result.Ok(patientsInfoDtos);
        }
    }
}
