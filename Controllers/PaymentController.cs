using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly RestaurantDbContext _context;

        public PaymentController(RestaurantDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<IActionResult> Details(int orderId)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderId == orderId);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }
    }
}
