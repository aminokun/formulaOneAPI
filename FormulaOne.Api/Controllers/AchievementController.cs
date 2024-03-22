using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FormulaOne.Entities.Dtos.Responses;
using FormulaOne.Entities.Dtos.Requests;
using FormulaOne.Entities.DbSet;

namespace FormulaOne.Api.Controllers
{
    public class AchievementController : BaseController
    {
        public AchievementController(
            IUnitOfWork unitOfWork, 
            IMapper mapper) : base(unitOfWork, mapper)
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
