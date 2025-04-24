using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication6.Data;
using WebApplication6.Dto;
using WebApplication6.Models;


namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        Appcontext context;
        public ReviewController(Appcontext context)
        {
            this.context = context;
        }
        ////////////////////////////////////////////////////////////
        [HttpPost("AddReview")]
        [Authorize]
        public IActionResult AddReview(ReviewDTO reviewFromUser)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Product product = context.Products.FirstOrDefault(p => p.ProductID == reviewFromUser.productId);
                if (product != null)
                {
                    //mapping
                    Review review = new Review();
                    review.Comment = reviewFromUser.Comment;
                    review.CreatedAt = DateTime.Now;
                    review.ProductId = reviewFromUser.productId;
                    review.UserId = userId;
                    //save review to database
                    context.reviews.Add(review);
                    context.SaveChanges();
                    return Ok(new { message = "review Added successfully" });
                }
                return NotFound("this product not found");
            }
            return BadRequest(ModelState);
        }
        ////////////////////////////////////////////////////////////
        [HttpPut("EditReview/{reviewId:int}")]
        [Authorize]
        public IActionResult EditReview(int reviewId, ReviewDTO reviewFromUser)
        {
            if (ModelState.IsValid)
            {
                // string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                /*Product product = context.product.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    product.Id = reviewFromUser.productId;
                    product.Reviews=reviewFromUser.Comment;
                }*/
                Review review = context.reviews.FirstOrDefault(r => r.Id == reviewId);
                if (review != null)
                {
                    review.ProductId = reviewFromUser.productId;
                    review.Comment = reviewFromUser.Comment;
                    context.reviews.Update(review);
                    context.SaveChanges();
                    return Ok(new { message = "Review updated successfully" });
                }
                return NotFound("reiew not found");
            }
            return BadRequest(ModelState);
        }
        ////////////////////////////////////////////////////////////
        [HttpDelete("DeleteReview/{reviewId:int}")]
        [Authorize]
        public IActionResult DeleteReview(int reviewId)
        {
            if (ModelState.IsValid)
            {
                Review review = context.reviews.FirstOrDefault(r => r.Id == reviewId);
                if (review != null)
                {
                    context.reviews.Remove(review);
                    context.SaveChanges();
                    return Ok(new { message = "Review deleted successfully.." });
                }
                return NotFound(new { message = "review not found" });
            }
            return BadRequest(ModelState);
        }
        ////////////////////////////////////////////////////////////
        [HttpGet("GetReview")]
        [Authorize]
        public IActionResult GetReview()
        {
            List<Review> reviews = context.reviews.Include(r => r.User).Include(r => r.Product).ToList();
            List<InformationReview> informationList = new List<InformationReview>();
            foreach (Review review in reviews)
            {
                InformationReview information = new InformationReview();
                information.CreatedAt = review.CreatedAt;
                information.UserName = review.User.UserName;
                information.Comment = review.Comment;
                information.ProductName = review.Product.Name;
                informationList.Add(information);

            }
            if (informationList.Count > 0)
            {
                return Ok(informationList);
            }
            return NotFound(new { message = "There is no reviews.." });

        }
    }
}
