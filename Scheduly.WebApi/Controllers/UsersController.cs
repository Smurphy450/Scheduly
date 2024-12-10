using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Models.DTO.User;
using Scheduly.WebApi.Utilities;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public UsersController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {
                var user = await _context.Users.FindAsync(updatePasswordDTO.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                user.PasswordHash = updatePasswordDTO.PasswordHash;

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = updatePasswordDTO.UserId,
                        Action = "UpdatePassword",
                        AffectedData = $"Updated password for user with ID: {updatePasswordDTO.UserId}"
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(updatePasswordDTO.UserId))
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
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, updatePasswordDTO.UserId, "UpdatePassword", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserSession>> AuthenticateUser([FromBody] UserAuthRequest request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == request.PasswordHash);

                if (user != null)
                {
                    var profiles = await _context.Profiles.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                    string role = profiles?.Admin ?? false ? "Administrator" : "User";

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = user.UserId,
                        Action = "AuthenticateUser",
                        AffectedData = $"Authenticated user with ID: {user.UserId}"
                    });

                    return Ok(new UserSession
                    {
                        Username = user.Username,
                        UserID = user.UserId,
                        Role = role
                    });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == request.PasswordHash);
                if (user != null)
                    await ErrorLoggingHelper.LogErrorAsync(_context, user.UserId, "AuthenticateUser", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromQuery] int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == id);
                if (profile != null)
                {
                    _context.Profiles.Remove(profile);
                }

                _context.Users.Remove(user);

            
                await _context.SaveChangesAsync();
                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = userId,
                    Action = "DeleteUser",
                    AffectedData = $"Deleted user with ID: {id}"
                });
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "DeleteUser", ex);
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                var user = await _context.Users.FindAsync(changePasswordDTO.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                if (user.PasswordHash != changePasswordDTO.OldPasswordHash)
                {
                    return BadRequest("Current password is incorrect.");
                }

                user.PasswordHash = changePasswordDTO.NewPasswordHash;

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = changePasswordDTO.UserId,
                        Action = "ChangePassword",
                        AffectedData = $"Changed password for user with ID: {changePasswordDTO.UserId}"
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(changePasswordDTO.UserId))
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
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, changePasswordDTO.UserId, "ChangePassword", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
