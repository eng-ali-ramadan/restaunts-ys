using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using System.Security.Claims;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly RestaurantDbContext _context;

        public ReviewController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int dishId, int rating, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (rating < 1 || rating > 5)
            {
                TempData["ErrorMessage"] = "Rating must be between 1 and 5 stars.";
                return RedirectToAction("Details", "Menu", new { id = dishId });
            }

            var review = new Review
            {
                DishId = dishId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your review has been submitted successfully!";
            return RedirectToAction("Details", "Menu", new { id = dishId });
        }
    }
}
