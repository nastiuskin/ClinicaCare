using Application.AppointmentManagement.Commands.Create;
using Application.AppointmentManagement.DTO;
using Application.MedicalProcedureManagement.Commands.Create;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("api/appointments")]

    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDto appointmentCreateDto)
        {
            var result = await _mediator.Send(new AppointmentCreateCommand(appointmentCreateDto));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }
    }
}
