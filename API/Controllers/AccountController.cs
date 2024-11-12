using Application.Auth.Login;
using Application.Auth.Logout;
using Application.Auth.RefreshToken;
using Application.Auth.Register;
using Application.UserAccountManagement.Doctors.Queries;
using Application.UserAccountManagement.UserDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> RegisterPatient([FromBody] UserFormDto patientDto)
        {
            var result = await _mediator.Send(new PatientRegisterCommand(patientDto));
            if (result.IsSuccess)
                return Ok("You have successfully registered");
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _mediator.Send(new UserLoginCommand(userLoginDto));
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _mediator.Send(new UserLogoutCommand());
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _mediator.Send(new RefreshTokenRotationCommand());
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("doctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginatedDoctors([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetAllDoctorsInfoQuery(pageNumber, pageSize));
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }

    }
}
