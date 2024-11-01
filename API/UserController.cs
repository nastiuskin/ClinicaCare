using Application.UserAccountManagement.Doctors.Commands.Create;
using Application.UserAccountManagement.Doctors.DTO;
using Application.UserAccountManagement.Doctors.Queries;
using Application.UserAccountManagement.Patients.Commands.Create;
using Application.UserAccountManagement.Patients.Queries;
using Application.UserAccountManagement.Patients.UserDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/doctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorFormDto doctorDto)
        {
            var result = await _mediator.Send(new DoctorCreateCommand(doctorDto));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("/doctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _mediator.Send(new GetAllDoctorsInfoQuery());
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("/patients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePatient([FromBody] UserFormDto patientDto)
        {
            var result = await _mediator.Send(new PatientCreateCommand(patientDto));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("/patients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await _mediator.Send(new GetAllPatientsInfoQuery());
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }
    }
}
