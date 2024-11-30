using Application.AppointmentManagement.DTO;
using Application.Auth;
using Application.Configuration.Queries;
using AutoMapper;
using Domain.Appointments;
using Domain.Helpers.PaginationStuff;
using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Application.AppointmentManagement.Queries
{
    public record GetAllAppointmentsByCurrentUserIdQuery(AppointmentParameters Parameters)
        : IQuery<Result<ICollection<AppointmentInfoDto>>>;

    public class GetAllAppointmentsByCurrentUserIdQueryHandler
        : IQueryHandler<GetAllAppointmentsByCurrentUserIdQuery, Result<ICollection<AppointmentInfoDto>>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;

        public GetAllAppointmentsByCurrentUserIdQueryHandler(
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor, 
            IJwtService jwtService)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
        }

        public async Task<Result<ICollection<AppointmentInfoDto>>> Handle(GetAllAppointmentsByCurrentUserIdQuery query,
            CancellationToken cancellationToken)
        {
            var userIdResult = _jwtService.GetUserIdFromTokenAsync(_httpContextAccessor);
            if (userIdResult.IsFailed)
                return Result.Fail(userIdResult.Errors);

            var userId = new UserId(userIdResult.Value);

            var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(roleClaim))
                return Result.Fail("User role claim is missing or invalid.");

            var appointments = _appointmentRepository.GetAppointmentsAsync(query.Parameters, roleClaim, userId);

            var appointmentDtos = _mapper.Map<ICollection<AppointmentInfoDto>>(appointments);
            _httpContextAccessor.HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(appointments.MetaData));


            return Result.Ok(appointmentDtos);
        }
    }
}
