using Application.AppointmentManagement.Commands.Create;
using Application.AppointmentManagement.DTO;
using Application.Auth;
using AutoMapper;
using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ClinicaCareTests.Appointments.Application.Tests.AppointmentHandlerTests.Commands
{
    public class AppointmentCreateCommandHandlerTests
    {
        private readonly AppointmentCreateCommandHandler _appointmentCreateCommandHandler;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMedicalProcedureRepository> _medicalProcedureRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<AppointmentFormDto>> _validatorMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        public AppointmentCreateCommandHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _medicalProcedureRepositoryMock = new Mock<IMedicalProcedureRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<AppointmentFormDto>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtServiceMock = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();

            _appointmentCreateCommandHandler = new AppointmentCreateCommandHandler(
                 _appointmentRepositoryMock.Object,
                 _mapperMock.Object,
                 _validatorMock.Object,
                 _userRepositoryMock.Object,
                 _medicalProcedureRepositoryMock.Object,
                 _httpContextAccessorMock.Object,
                 _jwtServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPatientIdNotFoundInToken()
        {
            var appointmentCreateDto = new AppointmentFormDto
            {
                DoctorId = Guid.NewGuid(),
                MedicalProcedureId = Guid.NewGuid(),
                Date = DateTime.Now.ToString(),
            };

            var command = new AppointmentCreateCommand(appointmentCreateDto);
            _jwtServiceMock.Setup(jwt => jwt.GetUserIdFromTokenAsync(It.IsAny<IHttpContextAccessor>()))
                .Returns(Result.Fail(new FluentResults.Error("User id claim is missing or invalid.")));

            var result = await _appointmentCreateCommandHandler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("User id claim is missing or invalid.", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenValidationFails()
        {
            var appointmentCreateDto = new AppointmentFormDto
            {
                PatientId = Guid.NewGuid()
            };

            var command = new AppointmentCreateCommand(appointmentCreateDto);

            _jwtServiceMock.Setup(jwt => jwt.GetUserIdFromTokenAsync(It.IsAny<IHttpContextAccessor>()))
                .Returns(Result.Ok(appointmentCreateDto.PatientId));

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AppointmentFormDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] {
                    new FluentValidation.Results.ValidationFailure("DoctorId", "Doctor cannot be null."),
                    new FluentValidation.Results.ValidationFailure("MedicalProcedureId", "Medical Procedure cannot be null."),
                    new FluentValidation.Results.ValidationFailure("Date", "Appointment date is required."),
                    new FluentValidation.Results.ValidationFailure("Duration.StartTime", "Appointment start time is required."),
                    new FluentValidation.Results.ValidationFailure("Duration.EndTime", "Appointment end time is required.")
       }));

            var result = await _appointmentCreateCommandHandler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Doctor cannot be null.", result.Errors[0].Message);
            Assert.Contains("Medical Procedure cannot be null.", result.Errors[1].Message);
            Assert.Contains("Appointment date is required.", result.Errors[2].Message);
            Assert.Contains("Appointment start time is required.", result.Errors[3].Message);
            Assert.Contains("Appointment end time is required.", result.Errors[4].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenMedicalProcedureNotFound()
        {
            var appointmentFormDto = new AppointmentFormDto
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                MedicalProcedureId = Guid.NewGuid(),
            };

            var command = new AppointmentCreateCommand(appointmentFormDto);

            _jwtServiceMock.Setup(jwt => jwt.GetUserIdFromTokenAsync(It.IsAny<IHttpContextAccessor>()))
                .Returns(Result.Ok(appointmentFormDto.PatientId));

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AppointmentFormDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _medicalProcedureRepositoryMock.Setup(mp => mp.GetByIdAsync(new MedicalProcedureId(appointmentFormDto.MedicalProcedureId))).ReturnsAsync((MedicalProcedure)null);

            var result = await _appointmentCreateCommandHandler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("Medical procedure not found.", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
        {
            var appointmentFormDto = new AppointmentFormDto
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                MedicalProcedureId = Guid.NewGuid(),
            };

            var command = new AppointmentCreateCommand(appointmentFormDto);

            _jwtServiceMock.Setup(jwt => jwt.GetUserIdFromTokenAsync(It.IsAny<IHttpContextAccessor>()))
                .Returns(Result.Ok(appointmentFormDto.PatientId));

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AppointmentFormDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var medicalProcedure = MedicalProcedure.Create(
                MedicalProcedureType.SURGERY,
                1900,
                TimeSpan.Parse("03:00"),
                "Procedure1");

            _medicalProcedureRepositoryMock.Setup(mp => mp.GetByIdAsync(It.IsAny<MedicalProcedureId>()))
                .ReturnsAsync(medicalProcedure.Value);

            _userRepositoryMock.Setup(d => d.GetByIdAsync(It.IsAny<UserId>()))
                .ReturnsAsync((Doctor)null);

            var result = await _appointmentCreateCommandHandler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("Doctor not found.", result.Errors.Select(e => e.Message).ToList());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAppointmentIsCreatedSuccessfully()
        {
            var appointmentFormDto = new AppointmentFormDto
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                MedicalProcedureId = Guid.NewGuid(),
                Duration = new TimeSlotDto(),
                Date = "04.12.2024"
            };

            var command = new AppointmentCreateCommand(appointmentFormDto);
            _jwtServiceMock.Setup(jwt => jwt.GetUserIdFromTokenAsync(It.IsAny<IHttpContextAccessor>()))
               .Returns(Result.Ok(appointmentFormDto.PatientId));

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AppointmentFormDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var medicalProcedure = MedicalProcedure.Create(
                MedicalProcedureType.SURGERY,
                1900,
                TimeSpan.Parse("03:00"),
                "Procedure1");

            _medicalProcedureRepositoryMock.Setup(mp => mp.GetByIdAsync(It.IsAny<MedicalProcedureId>()))
                .ReturnsAsync(medicalProcedure.Value);

            var doctor = Doctor.Create(new UserParams("Doctor", "Doctor", DateOnly.Parse("16.07.2002"), "doctor@gmail.com", "+37379155712"),
                new DoctorParams(SpecializationType.PEDIATRICIAN, "Bio", 100, TimeSpan.Parse("09:00"), TimeSpan.Parse("18:00")));

            _userRepositoryMock.Setup(mp => mp.GetByIdAsync(It.IsAny<UserId>()))
                .ReturnsAsync(doctor.Value);

            var result = await _appointmentCreateCommandHandler.Handle(command, default);

            Assert.True(result.IsSuccess);
            _appointmentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }
    }
}
