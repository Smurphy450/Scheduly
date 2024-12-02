using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSettingsController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public AdminSettingsController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/AdminSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminSetting>>> GetAdminSettings()
        {
            return await _context.AdminSettings.ToListAsync();
        }        

        // PUT: api/AdminSettings/UpdateList
        [HttpPut("UpdateList")]
        public async Task<IActionResult> PutAdminSettingsList(List<AdminSettingDTO> adminSettingsDto)
        {
            var adminSettings = adminSettingsDto.Select(dto => new AdminSetting
            {
                SettingsId = dto.SettingsId,
                Name = dto.Name,
                Enabled = dto.Enabled
            }).ToList();

            foreach (var adminSetting in adminSettings)
            {
                _context.Entry(adminSetting).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                foreach (var adminSetting in adminSettings)
                {
                    if (!AdminSettingExists(adminSetting.SettingsId))
                    {
                        return NotFound();
                    }
                }
                throw;
            }

            return NoContent();
        }

        private bool AdminSettingExists(int id)
        {
            return _context.AdminSettings.Any(e => e.SettingsId == id);
        }
    }
}
