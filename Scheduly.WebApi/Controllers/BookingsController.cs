﻿using System;
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
    public class BookingsController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public BookingsController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        [HttpGet("GetUserOverview/{userId}")]
        public async Task<ActionResult<UserOverviewDTO>> GetUserOverview(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Resource)
                .ThenInclude(r => r.Category)
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Premise)
                .ThenInclude(p => p.PremiseCategory)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            var overviewResources = user.Bookings
                .Where(b => b.ResourceId != null)
                .Select(b => new OverviewResourcesDTO
                {
                    CategoryName = b.Resource.Category.Name,
                    ResourceName = b.Resource.Name,
                    Description = b.Resource.Description,
                    Start = b.Start,
                    End = b.End ?? DateTimeOffset.MinValue,
                    Approved = b.Approved ?? false
                }).ToList();

            var overviewPremises = user.Bookings
                .Where(b => b.PremiseId != null)
                .Select(b => new OverviewPremisesDTO
                {
                    Name = b.Premise.Name,
                    CategoryName = b.Premise.PremiseCategory.Name,
                    Size = b.Premise.Size,
                    Start = b.Start,
                    End = b.End,
                    Approved = b.Approved
                }).ToList();

            var userOverview = new UserOverviewDTO
            {
                OverviewResources = overviewResources,
                OverviewPremises = overviewPremises
            };

            return Ok(userOverview);
        }

        // POST: api/Bookings/CreateBooking
        [HttpPost("CreateBooking")]
        public async Task<ActionResult<Booking>> CreateBooking(CreateBookingDTO createBookingDTO)
        {
            var booking = new Booking
            {
                UserId = createBookingDTO.UserId,
                PremiseId = createBookingDTO.PremiseId,
                ResourceId = createBookingDTO.ResourceId,
                Start = createBookingDTO.Start,
                End = createBookingDTO.End,
                Approved = createBookingDTO.Approved
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.BookingsId }, booking);
        }

        [HttpGet("ApproveBooking/{id}")]
        public async Task<ActionResult<ApproveBookingDTO>> GetApproveBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Premise)
                .ThenInclude(p => p.PremiseCategory)
                .Include(b => b.Resource)
                .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync(b => b.BookingsId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var approveBookingDTO = new ApproveBookingDTO
            {
                BookingId = booking.BookingsId,
                UserId = booking.UserId,
                Username = booking.User.Username,
                ItemName = booking.Premise != null ? booking.Premise.Name : booking.Resource.Name,
                CategoryName = booking.Premise != null ? booking.Premise.PremiseCategory.Name : booking.Resource.Category.Name,
                Start = booking.Start,
                End = booking.End
            };

            return Ok(approveBookingDTO);
        }
        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingsId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.BookingsId }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingsId == id);
        }
    }
}
