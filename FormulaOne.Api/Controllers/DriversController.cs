using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FormulaOne.Entities.Dtos.Requests;
using MediatR;
using FormulaOne.Api.Queries;
using FormulaOne.Api.Commands;

namespace FormulaOne.Api.Controllers
{
    public class DriversController : BaseController
    {

        public DriversController(
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IMapper mapper) : base(unitOfWork, mediator, mapper)
        {
        }

        [HttpGet]
        [Route("{driverId:guid}")]
        public async Task<IActionResult> GetDriver(Guid driverId)
        {
            var query = new GetDriverQuery(driverId);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            //Specifying the query that I have for this endpoint
            var query = new GetAllDriversQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddDriver([FromBody] CreateDriverRequest driver)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var command = new CreateDriverInfoRequest(driver);
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetDriver), new { driverId = result.DriverId }, result);
        }


        [HttpPut("")]
        public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverRequest driver)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var command = new UpdateDriverInfoRequest(driver);
            var result = await _mediator.Send(command);

            return result ? NoContent() : BadRequest();
        }


        [HttpDelete]
        [Route("{driverId:guid}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            var command = new DeleteDriverInfoRequest(driverId);
            var result = await _mediator.Send(command);

            return result ? NoContent() : BadRequest();
        }
    }
}
