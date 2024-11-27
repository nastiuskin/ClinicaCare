using Application.Configuration.Queries;
using Application.UserAccountManagement.Doctors.DTO;
using AutoMapper;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;

namespace Application.UserAccountManagement.Doctors.Queries
{
    public record GetListOfDoctorsQuery()
       : IQuery<Result<ICollection<DoctorPartialInfoDto>>>;

    public class GetListOfDoctorsQueryHandler
        : IQueryHandler<GetListOfDoctorsQuery, Result<ICollection<DoctorPartialInfoDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetListOfDoctorsQueryHandler(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<DoctorPartialInfoDto>>> Handle(GetListOfDoctorsQuery request,
            CancellationToken cancellationToken)
        {
            var doctors = _userRepository.GetAll().OfType<Doctor>().ToList();

            var doctorsInfoDtos = _mapper.Map<ICollection<DoctorPartialInfoDto>>(doctors);

            return Result.Ok(doctorsInfoDtos);
        }
    }
}

