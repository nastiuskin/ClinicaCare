using Application.AppointmentManagement.Queries;
using Application.Auth;
using AutoMapper;
using Domain.Appointments;
using Domain.Helpers.PaginationStuff;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace ClinicaCareTests.Appointments.Application.Tests.AppointmentHandlerTests.Queries
{
    public class GetAllAppointmentsByCurrentUserIdQueryHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly GetAllAppointmentsByCurrentUserIdQueryHandler _handler;

        public GetAllAppointmentsByCurrentUserIdQueryHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtServiceMock = new Mock<IJwtService>();

            _handler = new GetAllAppointmentsByCurrentUserIdQueryHandler(
                _appointmentRepositoryMock.Object,
                _mapperMock.Object,
                _httpContextAccessorMock.Object,
                _jwtServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserIdNotFoundInToken()
        {
            var query = new GetAllAppointmentsByCurrentUserIdQuery(new AppointmentParameters());

            _jwtServiceMock.Setup(jwt => jwt.GetUserIdFromTokenAsync(It.IsAny<IHttpContextAccessor>()))
                .Returns(Result.Fail(new FluentResults.Error("User id claim is missing or invalid.")));

            var result = await _handler.Handle(query, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("User id claim is missing or invalid.", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenRoleClaimIsNotFoundInToken()
        {
            var query = new GetAllAppointmentsByCurrentUserIdQuery(new AppointmentParameters());

            _jwtServiceMock.Setup(jwt => jwt.GetUserIdFromTokenAsync(It.IsAny<IHttpContextAccessor>()))
                .Returns(Guid.NewGuid());

            _httpContextAccessorMock
            .Setup(a => a.HttpContext.User.FindFirst(ClaimTypes.Role))
                .Returns(new Claim(ClaimTypes.Role, ""));

            var result = await _handler.Handle(query, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("User role claim is missing or invalid.", result.Errors[0].Message);
        }


    }
}
