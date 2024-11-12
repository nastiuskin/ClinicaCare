using Application.MedicalProcedureManagement.Queries;
using Domain.MedicalProcedures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/procedures")]
    public class MedicalProcedureController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MedicalProcedureController(IMediator mediator) => _mediator = mediator;

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
        public async Task<IActionResult> GetPaginatedMedicalProceduresInfo([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetAllMedicalProceduresInfoQuery(pageNumber, pageSize));
            if (result.IsSuccess) return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("by-type/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginatedMedicalProceduresByType(MedicalProcedureType type, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetAllMedicalProceduresInfoByTypeQuery(type, pageNumber, pageSize));
            if (result.IsSuccess) return Ok(result.Value);

            if (result.Errors.Any()) return BadRequest(result.Errors);
            return BadRequest(result.Errors);
        }


    }
}
