using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FormulaOne.Entities.Dtos.Responses;
using FormulaOne.Entities.Dtos.Requests;
using FormulaOne.Entities.DbSet;
using MediatR;

namespace FormulaOne.Api.Controllers
{
    public class AchievementsController : BaseController
    {
        public AchievementsController(
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IMapper mapper) : base(unitOfWork, mediator, mapper)
        {
        }

        [HttpGet]
        [Route("{driverId:guid}")]
        public async Task<IActionResult> GetDriverAchievement(Guid driverId)
        {
            var driverAchievements = await _unitOfWork.Achievements.GetDriverAchievementAsync(driverId);

            if (driverAchievements == null)
            {
                return NotFound("Achievement not found");
            }
            else
            {
                var result = _mapper.Map<DriverAchievementResponse>(driverAchievements);
                return Ok(result);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAchievement([FromBody] CreateDriverAchievementRequest achievement)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var result = _mapper.Map<Achievement>(achievement);
                
                await _unitOfWork.Achievements.Add(result);
                await _unitOfWork.CompleteAsync();
                
                return CreatedAtAction(
                    nameof(GetDriverAchievement), 
                    new { driverId = result.DriverId}, 
                    result
                    );
            }
        }
        
        [HttpPut("")]
        public async Task<IActionResult> UpdateAchievement([FromBody] UpdateDriverAchievementRequest achievement)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var result = _mapper.Map<Achievement>(achievement);

                await _unitOfWork.Achievements.Update(result);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
        }
    }
}
