using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BazarExam.Data;
using BazarExam.Models;

namespace BazarExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /api/sales
        [HttpPost]
        public async Task<IActionResult> AddSale([FromBody] Sale sale)
        {
            if (sale == null) return BadRequest();

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        // GET: /api/sales
        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            var sales = await _context.Sales.OrderByDescending(s => s.Date).ToListAsync();
            return Ok(sales);
        }
    }
}
