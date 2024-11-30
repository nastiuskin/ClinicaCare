using Application.AppointmentManagement.Commands.Cancel;
using Application.AppointmentManagement.Commands.Complete;
using Application.AppointmentManagement.Commands.Create;
using Application.AppointmentManagement.DTO;
using Application.AppointmentManagement.Queries;
using Domain.Helpers.PaginationStuff;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentFormDto appointmentCreateDto)
        {
            var result = await _mediator.Send(new AppointmentCreateCommand(appointmentCreateDto));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet]
        [Route("available-time-slots")]
        public async Task<IActionResult> GetAvailableTimeSlots([FromQuery] string doctorId, [FromQuery] string medicalProcedureId, [FromQuery] string date)
        {
            var result = await _mediator.Send(new GenerateAvailableTimeSlotsQuery(doctorId, medicalProcedureId, date));
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        [Route("{id}/complete")]
        public async Task<IActionResult> CompleteAppointment(Guid id)
        {
            var result = await _mediator.Send(new CompleteAppointmentCommand(id));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Patient, Doctor")]
        [HttpPost]
        [Route("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            var result = await _mediator.Send(new CancelAppointmentCommand(id));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        [Route("{id}/feedback")]
        public async Task<IActionResult> AddFeedbackToAppointment([FromRoute] Guid id, [FromBody] string Feedback)
        {
            var result = await _mediator.Send(new AddFeedbackToAppointmentCommand(id, Feedback));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPaginatedAppointmentsByCurrentUserId([FromQuery] AppointmentParameters parameters)
        {
            var result = await _mediator.Send(new GetAllAppointmentsByCurrentUserIdQuery(parameters));
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }
    }
}
