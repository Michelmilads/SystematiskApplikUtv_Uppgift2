using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;
using SystematiskApplikUtv_Uppgift2.Requests;

namespace SystematiskApplikUtv_Uppgift2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepo _ratingRepo;
        private readonly IRecipeRepo _recipeRepo;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IRatingRepo ratingRepo, IRecipeRepo recipeRepo, ILogger<RatingController> logger)
        {
            _ratingRepo = ratingRepo;
            _recipeRepo = recipeRepo;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateRating([FromQuery] NewRatingRequest request)
        {
            if (! ModelState.IsValid)
                return BadRequest();

            try
            {
                var recipe = _recipeRepo.GetRecipeThruID(request.RecipeID);
                if (recipe == null)
                    return NotFound();

                if (request.RatingValue < 1 || request.RatingValue > 5)
                    return BadRequest();

                var userRatings = _ratingRepo.GetRatingThruUserID(GetCurrentUser());
                if (userRatings.FirstOrDefault(r => r.RecipeID == recipe.RecipeID) != null)
                    return Forbid();

                if (recipe.UserID == GetCurrentUser())
                    return Forbid();

                Ratings newRating = new()
                {
                    RatingValue = request.RatingValue,
                    RecipeID = request.RecipeID,
                    RatedByUserID = GetCurrentUser()
                };

                _ratingRepo.CreateRating(newRating);
                return Ok("Rating Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Rating Could Not Be Created.");
            }
        }

        [HttpGet("recipe/{recipeID}")]
        [AllowAnonymous]
        public IActionResult GetRatingsthruRecipeID(int recipeID)
        {
            try
            {
                var ratings = _ratingRepo.GetRatingsThruRecipeID(recipeID);

                if (ratings.Count == 0)
                {
                    return NotFound("Error: The Recipe Does Not Have Any Existing Recipe's.");
                }

                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Ratings Could Not Be Fetched.");
            }
        }

        [HttpDelete("{ratingID}")]
        public IActionResult DeleteRating(int ratingID)
        {
            try
            {
                var rating = _ratingRepo.GetRatingThruRatingID(ratingID);
                if (rating == null)
                    return NotFound();

                if (rating.RatedByUserID != GetCurrentUser())
                    return Unauthorized();

                _ratingRepo.DeleteRating(ratingID);
                    return Ok("Successfully Deleted Rating.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                    return Problem(ex.Message);
            }
        }

        private int GetCurrentUser()
        {
            var idClaim = User.FindFirst("UserID");
            if (idClaim == null)
                return 0;

            var parsed = int.TryParse(idClaim.Value, out int id);
            if (parsed)
                return id;

            return 0;
        }
    }
}
