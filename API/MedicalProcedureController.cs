using Application.MedicalProcedureManagement.Commands.Create;
using Application.MedicalProcedureManagement.Commands.Delete;
using Application.MedicalProcedureManagement.Commands.Update;
using Application.MedicalProcedureManagement.DTO;
using Application.MedicalProcedureManagement.Queries;
using Domain.MedicalProcedures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.MedicalProcedures
{
    [ApiController]
    [Route("api/procedures")]
    public class MedicalProcedureController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MedicalProcedureController(IMediator mediator) => _mediator = mediator;


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMedicalProcedure([FromBody] MedicalProcedureFormDto procedureDto)
        {
            var result = await _mediator.Send(new CreateMedicalProcedureCommand(procedureDto));
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfoById(Guid id)
        {
            var result = await _mediator.Send(new GetOneMedicalProcedureInfoQuery(id));

            if (result.IsSuccess) return Ok(result.Value);
            return NotFound();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMedicalProceduresInfo()
        {
            var result = await _mediator.Send(new GetAllMedicalProceduresInfoQuery());
            if (result.IsSuccess) return Ok(result.Value);
            return BadRequest();
        }

        [HttpGet]
        [Route("by-type/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMedicalProceduresByType(MedicalProcedureType type)
        {
            var result = await _mediator.Send(new GetAllMedicalProceduresInfoByTypeQuery(type));
            if (result.IsSuccess) return Ok(result.Value);

            if (result.Errors.Any()) return BadRequest(result.Errors);
            return BadRequest();
        }

        [HttpPut]
        [Route("{id}")]
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
        [Route("{id}")]
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

        [HttpPost]
        [Route("{medicalId}/assign-doctor/{doctorId}")]
        public async Task<IActionResult> AssignDoctorToMedicalProcedure(Guid medicalId, Guid doctorId)
        {
            var result = await _mediator.Send(new AddDoctorToMedicalProcedureCommand(medicalId, doctorId));
            if (result.IsSuccess) return Ok();

            if (result.Errors.Any()) return BadRequest(result.Errors);
            return BadRequest();
        }

    }
}
