using Application.AppointmentManagement.Commands.Create;
using Application.AppointmentManagement.DTO;
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

        [HttpGet]
        [Route("timeSlots")]
        public async Task<IActionResult> GenerateAvailableTimeSlots([FromQuery] Guid doctorId, [FromQuery] Guid medicalProcedureId, [FromQuery] string date)
        {
            var result = await _mediator.Send(new GegerateAvailableTimeSlotsQuery(doctorId, medicalProcedureId, date));
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest();
        }
    }
}
