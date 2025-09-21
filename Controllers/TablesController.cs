using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Services;

namespace RestaurantBookingSystem.Controllers
{
    [ApiController]
    [Route("api/tables")]
    public class TablesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAvailabilityService _availabilityService;

        public TablesController(AppDbContext context, IAvailabilityService availabilityService)
        {
            _context = context;
            _availabilityService = availabilityService;
        }

        // GET: api/tables/available?date=2023-10-10&time=18:00&guests=4[&includeUnavailable=false]
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableTables(
            [FromQuery] DateTime date,
            [FromQuery] TimeSpan time,
            [FromQuery] int guests,
            [FromQuery] bool includeUnavailable = false)
        {
            // Capacity filter first
            var tables = await _context.Tables
                .Where(t => t.Capacity >= guests)
                .ToListAsync();

            var results = new List<object>();

            // Check availability SEQUENTIALLY (not in parallel)
            foreach (var table in tables)
            {
                var availability = await _availabilityService.CheckTableAvailabilityAsync(table.Id, date, time, guests);

                if (includeUnavailable || availability.IsAvailable)
                {
                    results.Add(new
                    {
                        table.Id,
                        table.TableNumber,
                        table.Capacity,
                        isAvailable = availability.IsAvailable,
                        reason = availability.IsAvailable ? "Available" : availability.Reason
                    });
                }
            }

            return Ok(results);
        }



        // GET: api/tables/available
        [HttpGet("{tableId}/available")]
        public async Task<IActionResult> IsTableAvailable(int tableId, DateTime date, TimeSpan time, int guests)
        {
            var availability = await _availabilityService.CheckTableAvailabilityAsync(tableId, date, time, guests);

            return Ok(new
            {
                isAvailable = availability.IsAvailable,
                reason = availability.Reason
            });
        }

        // GET: api/tables
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTables()
        {
            var tables = await _context.Tables.ToListAsync();
            return Ok(tables);
        }

        // GET api/tables/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTable(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null) return NotFound();
            return Ok(table);
        }

        // POST api/tables
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostTable(Table table)
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTable), new { id = table.Id }, table);
        }

        // PUT api/tables/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTable(int id, Table table)
        {
            if (id != table.Id) return BadRequest();
            _context.Entry(table).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tables.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE api/tables/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null) return NotFound();
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}