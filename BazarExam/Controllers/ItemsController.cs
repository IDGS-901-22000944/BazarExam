using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BazarExam.Data;
using BazarExam.Models;

namespace BazarExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/items?q=perfume
        [HttpGet]
        public async Task<IActionResult> SearchItems([FromQuery] string? q)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(p => p.Title.ToLower().Contains(q.ToLower()));
            }

            var items = await query.ToListAsync();
            return Ok(new { count = items.Count, products = items });
        }

        // GET: /api/items/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
