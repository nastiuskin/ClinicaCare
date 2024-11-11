using Application.AppointmentManagement.DTO;
using Application.Auth;
using Application.Configuration.Commands;
using AutoMapper;
using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.AppointmentManagement.Commands.Create
{
    public record AppointmentCreateCommand(AppointmentFormDto AppointmentCreateDto)
        : ICommand<Result>;

    public class AppointmentCreateCommandHandler
        : ICommandHandler<AppointmentCreateCommand, Result>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AppointmentFormDto> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;

        public AppointmentCreateCommandHandler(
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IValidator<AppointmentFormDto> validator,
            IUserRepository userRepository,
            IMedicalProcedureRepository medicalProcedureRepository,
            IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _validator = validator;
            _userRepository = userRepository;
            _medicalProcedureRepository = medicalProcedureRepository;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
        }

        public async Task<Result> Handle(AppointmentCreateCommand request,
            CancellationToken cancellationToken)
        { 
            var patientIdResult = _jwtService.GetUserIdFromTokenAsync(_httpContextAccessor);
            if (patientIdResult.IsFailed)
                return Result.Fail(patientIdResult.Errors);

            request.AppointmentCreateDto.PatientId = patientIdResult.Value;

            var validationResult = await _validator.ValidateAsync(request.AppointmentCreateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var medicalProcedure = await _medicalProcedureRepository.GetByIdAsync(new MedicalProcedureId(request.AppointmentCreateDto.MedicalProcedureId));
            if (medicalProcedure == null)
                return Result.Fail(new FluentResults.Error("Medical procedure not found."));

            var doctor = await _userRepository.GetByIdAsync(new UserId(request.AppointmentCreateDto.DoctorId));
            if (doctor == null)
                return Result.Fail(new FluentResults.Error("Doctor not found."));

            var appointment = _mapper.Map<Appointment>(request.AppointmentCreateDto);
            await _appointmentRepository.AddAsync(appointment);
            return Result.Ok();
        }
    }
}
