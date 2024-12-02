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
    public class ZipCodesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public ZipCodesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/ZipCodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZipCode>>> GetZipCodes()
        {
            return await _context.ZipCodes.ToListAsync();
        }

        // GET: api/ZipCodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZipCode>> GetZipCode(int id)
        {
            var zipCode = await _context.ZipCodes.FindAsync(id);

            if (zipCode == null)
            {
                return NotFound();
            }

            return zipCode;
        }
    }
}
