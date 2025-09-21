using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.DTOs.Requests;
using RestaurantBookingSystem.Models;

namespace RestaurantBookingSystem.Controllers
{
    // Controllers/DishesController.cs
    [ApiController]
    [Route("api/dishes")]
    public class DishesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DishesController(AppDbContext context) => _context = context;

        // GET: api/dishes
        [HttpGet]
        public async Task<IActionResult> GetDishes()
        {
            var dishes = await _context.Dishes.ToListAsync();
            return Ok(dishes);
        }

        // GET api/dishes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            return Ok(dish);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostDish([FromBody] CreateDishRequest dishDto)
        {
            var dish = new Dish
            {
                Name = dishDto.Name,
                Price = dishDto.Price,
                Description = dishDto.Description,
                IsPopular = dishDto.IsPopular,
                ImageUrl = dishDto.ImageUrl
                // Id is not set - let database handle it
            };

            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
        }

        // PUT api/dishes/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.Id) return BadRequest();
            _context.Entry(dish).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Dishes.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE api/dishes/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
