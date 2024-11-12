using Application.AppointmentManagement.DTO;
using Application.Configuration.Queries;
using AutoMapper;
using Domain.Appointments;
using Domain.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.AppointmentManagement.Queries
{
    public record GetAllAppointmentsByPatientIdQuery(Guid PatientId, int PageNumber, int PageSize)
        : IQuery<Result<ICollection<AppointmentInfoDto>>>;

    public class GetAllAppointmentByPatientIdQueryHandler
        : IQueryHandler<GetAllAppointmentsByPatientIdQuery, Result<ICollection<AppointmentInfoDto>>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        public GetAllAppointmentByPatientIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<AppointmentInfoDto>>> Handle(GetAllAppointmentsByPatientIdQuery query,
            CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetPaginatedAsync(query.PageNumber, query.PageSize)
                 .Where(a => a.PatientId == new UserId(query.PatientId)).ToListAsync();

            var appointmentDtos = _mapper.Map<ICollection<AppointmentInfoDto>>(appointments);

            return Result.Ok(appointmentDtos);
        }
    }
}
