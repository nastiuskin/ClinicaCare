using Application.Configuration.Queries;
using Application.SeedOfWork;
using Domain.DomainServices;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.AppointmentManagement.Commands.Create
{
    public record GenerateAvailableTimeSlotsQuery(Guid DoctorId, Guid MedicalProcedureId, string Date)
        : IQuery<Result<List<TimeSlot>>>;

    public class GenerateAvailableTimeSlotsQueryHandler
        : IQueryHandler<GenerateAvailableTimeSlotsQuery, Result<List<TimeSlot>>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IUserRepository _userRepository;

        public GenerateAvailableTimeSlotsQueryHandler(IMedicalProcedureRepository medicalProcedureRepository, IUserRepository userRepository)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<List<TimeSlot>>> Handle(GenerateAvailableTimeSlotsQuery request, CancellationToken cancellationToken)
        {
            var medicalProcedure = await _medicalProcedureRepository.GetByIdAsync(new MedicalProcedureId(request.MedicalProcedureId));
            if (medicalProcedure == null) return Result.Fail(ResponseError.NotFound(nameof(medicalProcedure), request.MedicalProcedureId));

            var doctor = await _userRepository.GetByIdWithAppointmentsOnSpecificDayAsync(new UserId(request.DoctorId), DateOnly.Parse(request.Date)).FirstOrDefaultAsync();

            if (doctor == null) return Result.Fail(ResponseError.NotFound(nameof(doctor), request.DoctorId));

            var availableTimeSlots = AvailableTimeSlotService.GetAvailableTimeSlotsForDay(doctor, medicalProcedure, DateOnly.Parse(request.Date)).Value;
            return Result.Ok(availableTimeSlots);
        }
    }
}
