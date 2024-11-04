using Application.AppointmentManagement.DTO;
using Application.Configuration.Queries;
using AutoMapper;
using Domain.Appointments;
using Domain.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.AppointmentManagement.Queries
{
    public record GetAllAppointmentsByDoctorIdQuery(Guid DoctorId, int PageNumber, int PageSize)
        : IQuery<Result<ICollection<AppointmentInfoDto>>>;

    public class GetAllAppointmentsByDoctorIdQueryHandler
        : IQueryHandler<GetAllAppointmentsByDoctorIdQuery, Result<ICollection<AppointmentInfoDto>>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        public GetAllAppointmentsByDoctorIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<AppointmentInfoDto>>> Handle(GetAllAppointmentsByDoctorIdQuery query,
            CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllAsync(query.PageNumber, query.PageSize)
                 .Where(a => a.DoctorId == new UserId(query.DoctorId)).ToListAsync();

            var appointmentDtos = _mapper.Map<ICollection<AppointmentInfoDto>>(appointments);

            return Result.Ok(appointmentDtos);
        }
    }
}
