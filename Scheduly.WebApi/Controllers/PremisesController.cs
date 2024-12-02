using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremisesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public PremisesController(SchedulyContext context)
        {
            _context = context;
        }      

        // GET: api/Premises/Category/{premisecategoryId}
        [HttpGet("Category/{premisecategoryId}")]
        public async Task<ActionResult<IEnumerable<Premise>>> GetResourcesByCategory(int premisecategoryId)
        {
            var category = await _context.Resources.FindAsync(premisecategoryId);

            if (category == null)
            {
                return NotFound();
            }

            return await _context.Premises
                .Where(r => r.PremiseCategoryId == premisecategoryId)
                .ToListAsync();
        }

        [HttpPost("CreatePremise")]
        public async Task<ActionResult<Premise>> CreateResource([FromBody] CreatePremiseDTO premiseDTO)
        {
            if (!string.IsNullOrEmpty(premiseDTO.Name))
            {
                var premise = new Premise
                {
                    PremiseCategoryId = premiseDTO.PremiseCategoryId,
                    Name = premiseDTO.Name,
                    Size = premiseDTO.Size,
                    Description = premiseDTO.Description,
                    MustBeApproved = premiseDTO.MustBeApproved,
                };

                _context.Premises.Add(premise);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPremise", new { id = premise.PremiseCategoryId }, premise);
            }
            else
            {
                return BadRequest("Name is required");
            }
        }


        // DELETE: api/Premises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePremise(int id)
        {
            var premise = await _context.Premises.FindAsync(id);
            if (premise == null)
            {
                return NotFound();
            }

            _context.Premises.Remove(premise);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
