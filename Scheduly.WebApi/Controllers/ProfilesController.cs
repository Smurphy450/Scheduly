using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Models.DTO.User;
using Scheduly.WebApi.Utilities;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public ProfilesController(SchedulyContext context)
        {
            _context = context;
        }
        
        [HttpGet("UserDto/{userId}")] 
        public async Task<ActionResult<ProfileDTO>> GetProfileDTOByUserId(int userId)
        {
            try
            {
                var profile = await _context.Profiles
                .Where(a => a.UserId == userId)
                .Select(p => new ProfileDTO
                {
                    UserId = p.UserId ?? 0,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Address = p.Address,
                    ZipCode = p.ZipCode ?? 0,
                    City = p.ZipCodeNavigation.City,
                    PhoneNumber = p.PhoneNumber,
                    Username = p.User.Username,
                    Email = p.User.Email
                })
                .FirstOrDefaultAsync();

                if (profile == null)
                {
                    return NotFound();
                }

                return profile;
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 1, "GetProfileDTOByUserId", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Profiles/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<Profile>> GetProfileByUserId(int userId)
        {
            try
            {
                var profile = await _context.Profiles
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync();

                if (profile == null)
                {
                    return NotFound();
                }

                return profile;
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 1, "GetProfileByUserId", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Profiles/UserProfile/{userId}
        [HttpGet("UserProfile/{userId}")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfileByUserId(int userId)
        {
            try
            {
                var profile = await _context.Profiles
                .Include(p => p.User)
                .Where(a => a.UserId == userId)
                .Select(p => new UserProfileDTO
                {
                    UserId = p.UserId ?? 0,
                    Username = p.User.Username,
                    Email = p.User.Email,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Address = p.Address,
                    ZipCode = p.ZipCode,
                    PhoneNumber = p.PhoneNumber,
                    Admin = p.Admin,
                    City = p.ZipCodeNavigation.City
                })
                .FirstOrDefaultAsync();

                if (profile == null)
                {
                    return NotFound();
                }

                return profile;
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 1, "GetUserProfileByUserId", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Profiles/UserProfile
        [HttpPut("UserProfile")]
        public async Task<IActionResult> PutUserProfileByUserId(UserProfileDTO userProfileDto, [FromQuery] int userId)
        {
            try
            {
                var profile = await _context.Profiles
                .Include(p => p.User) // Ensure User is included
                .FirstOrDefaultAsync(p => p.UserId == userProfileDto.UserId);
                if (profile == null)
                {
                    return NotFound();
                }

                // Update profile properties
                profile.FirstName = userProfileDto.FirstName;
                profile.LastName = userProfileDto.LastName;
                profile.Address = userProfileDto.Address;
                profile.ZipCode = userProfileDto.ZipCode;
                profile.PhoneNumber = userProfileDto.PhoneNumber;
                profile.Admin = userProfileDto.Admin;

                // Update user properties
                profile.User.Username = userProfileDto.Username;
                profile.User.Email = userProfileDto.Email;

                // Update password hash if provided
                if (!string.IsNullOrEmpty(userProfileDto.PasswordHash))
                {
                    profile.User.PasswordHash = userProfileDto.PasswordHash;
                }

                _context.Entry(profile).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = userId,
                        Action = "PutUserProfileByUserId",
                        AffectedData = $"Updated profile for user with ID: {userProfileDto.UserId}"
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ProfileId))
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
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "PutUserProfileByUserId", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("User")]
        public async Task<IActionResult> PutProfileByUserId(ProfileDTO profileDto, [FromQuery] int userId)
        {
            try
            {
                var profile = await _context.Profiles
                .Include(p => p.User) // Ensure User is included
                .FirstOrDefaultAsync(p => p.UserId == profileDto.UserId);
                if (profile == null)
                {
                    return NotFound();
                }

                profile.FirstName = profileDto.FirstName;
                profile.LastName = profileDto.LastName;
                profile.Address = profileDto.Address;
                profile.ZipCode = profileDto.ZipCode;
                profile.PhoneNumber = profileDto.PhoneNumber;
                profile.User.Username = profileDto.Username;
                profile.User.Email = profileDto.Email;

                _context.Entry(profile).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = userId,
                        Action = "PutProfileByUserId",
                        AffectedData = $"Updated profile for user with ID: {profileDto.UserId}"
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ProfileId))
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
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "PutProfileByUserId", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Profiles/UserProfile
        [HttpPost("UserProfile")]
        public async Task<ActionResult<UserProfileDTO>> PostUserProfile(UserProfileDTO userProfileDto, [FromQuery] int userId)
        {
            try
            {
                if (userProfileDto == null || string.IsNullOrEmpty(userProfileDto.Username) || string.IsNullOrEmpty(userProfileDto.Email) || string.IsNullOrEmpty(userProfileDto.PasswordHash))
                {
                    return BadRequest("Invalid user profile data.");
                }

                var user = new User
                {
                    Username = userProfileDto.Username,
                    Email = userProfileDto.Email,
                    PasswordHash = userProfileDto.PasswordHash
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var profile = new Profile
                {
                    UserId = user.UserId,
                    FirstName = userProfileDto.FirstName,
                    LastName = userProfileDto.LastName,
                    Address = userProfileDto.Address,
                    ZipCode = userProfileDto.ZipCode,
                    PhoneNumber = userProfileDto.PhoneNumber,
                    Admin = userProfileDto.Admin
                };

                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();

                // Create a notification for the new user
                var notification = new Notification
                {
                    UserId = user.UserId,
                    Sms = true,
                    Email = true,
                    Message = $"New password: '{userProfileDto.Password}' for Scheduly" // Use the unhashed password
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = userId,
                    Action = "PostUserProfile",
                    AffectedData = $"Created profile for user with ID: {user.UserId}"
                });

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "PostUserProfile", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.ProfileId == id);
        }
    }
}
