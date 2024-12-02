using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;

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

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserSession>> AuthenticateUser([FromForm] UserAuthRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == request.PasswordHash);

            if (user != null)
            {
                var profiles = await _context.Profiles.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                //var profile = await _context.Profiles.FindAsync(user.UserId);
                string role = profiles?.Admin ?? false ? "Administrator" : "User";

                return Ok(new UserSession
                {
                    Username = user.Username,
                    UserID = user.UserId,
                    Role = role
                });
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
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

            return NoContent();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
