using Application.MedicalProcedureManagement.Commands.Create;
using Application.MedicalProcedureManagement.Commands.Delete;
using Application.MedicalProcedureManagement.Commands.Update;
using Application.MedicalProcedureManagement.DTO;
using Application.MedicalProcedureManagement.Queries;
using Domain.MedicalProcedures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.MedicalProcedures
{
    [ApiController]
    [Route("api/procedures")]
    public class MedicalProcedureController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MedicalProcedureController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> GetAllMedicalProceduresInfo([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetAllMedicalProceduresInfoQuery(pageNumber, pageSize));
            if (result.IsSuccess) return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("by-type/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMedicalProceduresByType(MedicalProcedureType type, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetAllMedicalProceduresInfoByTypeQuery(type, pageNumber, pageSize));
            if (result.IsSuccess) return Ok(result.Value);

            if (result.Errors.Any()) return BadRequest(result.Errors);
            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
    }
}
