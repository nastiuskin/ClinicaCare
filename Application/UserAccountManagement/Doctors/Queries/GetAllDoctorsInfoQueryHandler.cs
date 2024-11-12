using Application.Configuration.Queries;
using Application.UserAccountManagement.Doctors.DTO;
using AutoMapper;
using Domain.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.UserAccountManagement.Doctors.Queries
{
    public record GetAllDoctorsInfoQuery(int PageNumber, int PageSize)
        : IQuery<Result<ICollection<DoctorPartialInfoDto>>>;

    public class GetAllDoctorsInfoQueryHandler
        : IQueryHandler<GetAllDoctorsInfoQuery, Result<ICollection<DoctorPartialInfoDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllDoctorsInfoQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<DoctorPartialInfoDto>>> Handle(GetAllDoctorsInfoQuery request,
            CancellationToken cancellationToken)
        {
            var doctors = await _userRepository.GetPaginatedDoctorsAsync(request.PageNumber, request.PageSize).ToListAsync();

            var doctorsInfoDtos = _mapper.Map<ICollection<DoctorPartialInfoDto>>(doctors);

            return Result.Ok(doctorsInfoDtos);
        }
    }
}
