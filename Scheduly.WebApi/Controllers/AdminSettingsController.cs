using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApi.Utilities;

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
            var userId = adminSettingsDto.FirstOrDefault()?.UserId ?? 0;
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

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = userId,
                    Action = "UpdateAdminSettings",
                    AffectedData = "Updated admin settings list"
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var adminSetting in adminSettings)
                {
                    if (!AdminSettingExists(adminSetting.SettingsId))
                    {
                        return NotFound();
                    }
                }

                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "UpdateAdminSettings", ex);
                throw;
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "UpdateAdminSettings", ex);
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        private bool AdminSettingExists(int id)
        {
            return _context.AdminSettings.Any(e => e.SettingsId == id);
        }
    }
}
