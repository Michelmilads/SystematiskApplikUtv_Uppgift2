using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepo _ratingRepo;

        public RatingController(IRatingRepo ratingRepo)
        {
            _ratingRepo = ratingRepo;
        }

        [HttpPost]
        public IActionResult CreateRating([FromBody] Ratings ratings)
        {
            if (ratings == null)
            {
                return BadRequest("Error: Rating Data Invalid.");
            }
            if (ratings.RatingValue > 5 || ratings.RatingValue < 1) //Kollar om betyget är mellan 1 & 5
            {
                return BadRequest("Error: Rating Data Invalid.");
            }


            try
            {
                var tokenUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Check if you've already rated the recipe before
                var checkIfPreviousRating = _ratingRepo.GetRatingThruUserID(tokenUserID);
                if (checkIfPreviousRating != null) //!rating.RatingValue.HasValue
                {
                    return Forbid("This Recipe Has Been Already Been rated By You.");
                }

                // Check the recipe being rated are not your own
                if (_ratingRepo.CheckIfUserIsOwner(ratings.RecipeID, tokenUserID).Any())
                {
                    return Forbid("You Are Not Allowed To Rate Your Own Recipe.");
                }


                _ratingRepo.CreateRating(ratings);
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

        [HttpPatch("{ratingID}")]
        public IActionResult UpdateRating(int ratingID, [FromBody] Ratings updateRating)
        {
            if (updateRating == null)
            {
                return BadRequest("Error: Rating Data Invalid");
            }

            try
            {
                var ratedThruUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                updateRating.RatedByUserID = ratedThruUserID;

                if (_ratingRepo.GetRatingThruRatingID(ratingID).RatedByUserID != ratedThruUserID)
                {
                    return Forbid("Not Authorized To Update This Recipe Since You Are Not The Owner.");
                }


                _ratingRepo.UpdateRatings(ratingID, updateRating);
                return Ok("Successfully Updated Rating.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Update The Rating.");
            }
        }

        [HttpDelete("{ratingID}")]
        public IActionResult DeleteRating(int ratingID)
        {
            try
            {
                var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (_ratingRepo.GetRatingThruRatingID(ratingID).RatedByUserID != userID)
                {
                    return Forbid("Not Authorized To Delete This Rating Since You Are Not The Owner.");
                }

                _ratingRepo.DeleteRating(ratingID);
                return Ok("Successfully Deleted Rating.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Delete The Rating.");
            }
        }
    }
}
