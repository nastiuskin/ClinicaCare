using Application.MedicalProcedureManagement.Commands.Create;
using Application.MedicalProcedureManagement.Commands.Delete;
using Application.MedicalProcedureManagement.Commands.Update;
using Application.MedicalProcedureManagement.DTO;
using Application.UserAccountManagement.Doctors.Commands.Create;
using Application.UserAccountManagement.Doctors.DTO;
using Application.UserAccountManagement.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("doctors")]
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
        [Route("patients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginatedPatients([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetAllPatientsInfoQuery(pageNumber, pageSize));
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("procedures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMedicalProcedure([FromBody] MedicalProcedureFormDto procedureDto)
        {
            var result = await _mediator.Send(new CreateMedicalProcedureCommand(procedureDto));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [HttpPut]
        [Route("procedures/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMedicalProcedure(Guid id, [FromBody] MedicalProcedureUpdateDto updateMedicalProcedureDto)
        {
            var result = await _mediator.Send(new UpdateMedicalProcedureCommand(id, updateMedicalProcedureDto));

            if (result.IsSuccess) return Ok(result.Value);

            if (result.Errors.Any()) return BadRequest(result.Errors);
            return NotFound();
        }

        [HttpDelete]
        [Route("procedures/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMedicalProcedure(Guid id)
        {
            var result = await _mediator.Send(new DeleteMedicalProcedureCommand(id));
            if (result.IsSuccess) return Ok();

            if (result.Errors.Any()) return BadRequest(result.Errors);
            return NotFound();
        }
    }
}
