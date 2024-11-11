using Application.AppointmentManagement.DTO;
using Application.Configuration.Queries;
using Application.SeedOfWork;
using Domain.DomainServices;
using Domain.MedicalProcedures;
using Domain.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.AppointmentManagement.Commands.Create
{
    public record GenerateAvailableTimeSlotsQuery(Guid DoctorId, Guid MedicalProcedureId, string Date)
        : IQuery<Result<List<TimeSlotDto>>>;

    public class GenerateAvailableTimeSlotsQueryHandler
        : IQueryHandler<GenerateAvailableTimeSlotsQuery, Result<List<TimeSlotDto>>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IUserRepository _userRepository;

        public GenerateAvailableTimeSlotsQueryHandler(
            IMedicalProcedureRepository medicalProcedureRepository,
            IUserRepository userRepository)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<List<TimeSlotDto>>> Handle(GenerateAvailableTimeSlotsQuery request, CancellationToken cancellationToken)
        {
            var medicalProcedure = await _medicalProcedureRepository
                .GetByIdAsync(new MedicalProcedureId(request.MedicalProcedureId));
            if (medicalProcedure == null) 
                return Result.Fail(ResponseError.NotFound(nameof(medicalProcedure), request.MedicalProcedureId));

            var doctor = await _userRepository.GetByIdWithAppointmentsOnSpecificDayAsync(
                new UserId(request.DoctorId), DateOnly.Parse(request.Date)).FirstOrDefaultAsync();

            if (doctor == null) 
                return Result.Fail(ResponseError.NotFound(nameof(doctor), request.DoctorId));

            var availableTimeSlots = AvailableTimeSlotService.GetAvailableTimeSlotsForDay(
                doctor, medicalProcedure, DateOnly.Parse(request.Date)).Value;

            var timeSlotDtos = availableTimeSlots.Select(slot => new TimeSlotDto
            {
                StartTime = slot.StartTime.ToString(@"hh\:mm"),
                EndTime = slot.EndTime.ToString(@"hh\:mm")
            }).ToList();

            return Result.Ok(timeSlotDtos);
        }
    }
}
