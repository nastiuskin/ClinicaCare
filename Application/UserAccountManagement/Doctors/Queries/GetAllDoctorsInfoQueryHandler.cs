using Application.Configuration.Queries;
using Application.UserAccountManagement.Doctors.DTO;
using AutoMapper;
using Domain.Helpers.PaginationStuff;
using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.UserAccountManagement.Doctors.Queries
{
    public record GetAllDoctorsInfoQuery(DoctorParameters Parameters)
        : IQuery<Result<ICollection<DoctorPartialInfoDto>>>;

    public class GetAllDoctorsInfoQueryHandler
        : IQueryHandler<GetAllDoctorsInfoQuery, Result<ICollection<DoctorPartialInfoDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllDoctorsInfoQueryHandler(IUserRepository userRepository, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<ICollection<DoctorPartialInfoDto>>> Handle(GetAllDoctorsInfoQuery request,
            CancellationToken cancellationToken)
        {
            var doctors = _userRepository.GetAllDoctorsAsync(request.Parameters);

            var doctorsInfoDtos = _mapper.Map<ICollection<DoctorPartialInfoDto>>(doctors);
            _httpContextAccessor.HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(doctors.MetaData));


            return Result.Ok(doctorsInfoDtos);
        }
    }
}
