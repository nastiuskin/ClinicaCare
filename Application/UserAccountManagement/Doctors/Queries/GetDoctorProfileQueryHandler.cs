using Application.Configuration.Queries;
using Application.UserAccountManagement.UserDtos;
using AutoMapper;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;
using Shared.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccountManagement.Doctors.Queries
{
    public record GetDoctorProfileQuery(Guid id)
    : IQuery<Result<DoctorViewDto>>;

    public class GetDoctorProfileQueryHandler
        : IQueryHandler<GetDoctorProfileQuery, Result<DoctorViewDto>>
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        public GetDoctorProfileQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<DoctorViewDto>> Handle(GetDoctorProfileQuery query, CancellationToken cancellationToken)
        {
            var doctor = await _userRepository.GetByIdAsync(new UserId(query.id));
            if (doctor == null)
                return Result.Fail(new FluentResults.Error("Doctor does not exist"));

            var userViewDto = _mapper.Map<DoctorViewDto>((Doctor)doctor);
            return Result.Ok(userViewDto);
        }
    }
}
