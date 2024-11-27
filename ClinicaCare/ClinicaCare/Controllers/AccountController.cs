using Application.Auth.Login;
using Application.Auth.Logout;
using Application.Auth.RefreshToken;
using Application.Auth.Register;
using Application.UserAccountManagement;
using Application.UserAccountManagement.Doctors.Queries;
using Application.UserAccountManagement.UserDtos;
using Domain.Helpers.PaginationStuff;
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
        public async Task<IActionResult> GetPaginatedDoctors([FromQuery] DoctorParameters doctorParameters)
        {
            var result = await _mediator.Send(new GetAllDoctorsInfoQuery(doctorParameters));
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }


        [HttpGet]
        [Route("doctors-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _mediator.Send(new GetListOfDoctorsQuery());
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        //[Authorize]
        [HttpGet]
        [Route("profile")]
        public async Task<ActionResult> GetUserProfile()
        {
            var result = await _mediator.Send(new GetUserProfileQuery());
            if(result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPut]
        [Route("profile/edit")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserViewDto userFormDto)
        {
            var result = await _mediator.Send(new UpdateUserCommand(userFormDto));
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("{id}/doctor-profile")]
        public async Task<ActionResult> GetDoctorProfile(Guid id)
        {
            var result = await _mediator.Send(new GetDoctorProfileQuery(id));
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }
    }
}
