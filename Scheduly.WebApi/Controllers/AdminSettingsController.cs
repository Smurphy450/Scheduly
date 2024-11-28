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

        // GET: api/AdminSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminSetting>> GetAdminSetting(int id)
        {
            var adminSetting = await _context.AdminSettings.FindAsync(id);

            if (adminSetting == null)
            {
                return NotFound();
            }

            return adminSetting;
        }

        // PUT: api/AdminSettings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminSetting(int id, AdminSetting adminSetting)
        {
            if (id != adminSetting.SettingsId)
            {
                return BadRequest();
            }

            _context.Entry(adminSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminSettingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/AdminSettings/UpdateList
        [HttpPut("UpdateList")]
        public async Task<IActionResult> PutAdminSettingsList(List<AdminSettingDto> adminSettingsDto)
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

        // POST: api/AdminSettings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminSetting>> PostAdminSetting(AdminSetting adminSetting)
        {
            _context.AdminSettings.Add(adminSetting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdminSetting", new { id = adminSetting.SettingsId }, adminSetting);
        }

        // DELETE: api/AdminSettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminSetting(int id)
        {
            var adminSetting = await _context.AdminSettings.FindAsync(id);
            if (adminSetting == null)
            {
                return NotFound();
            }

            _context.AdminSettings.Remove(adminSetting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminSettingExists(int id)
        {
            return _context.AdminSettings.Any(e => e.SettingsId == id);
        }
    }
}
