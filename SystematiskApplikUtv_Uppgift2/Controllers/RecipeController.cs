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
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepo _recipeRepo;

        public RecipeController(IRecipeRepo recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }


        [HttpPost]
        public IActionResult CreateRecipe([FromBody] Recipe recipe)
        {
            if (recipe == null)
            {
                return BadRequest("Error: Invalid Recipe Data.");
            }

            try
            {
                var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                recipe.UserID = userID;

                _recipeRepo.CreateRecipe(recipe);
                return StatusCode(StatusCodes.Status201Created, "The Recipe Has Been Created.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Create The Recipe.");
            }
        }

        [HttpGet("{recipeID}")]
        [AllowAnonymous]
        public IActionResult GetRecipeThruID(int recipeID)
        {
            try
            {
                var recipe = _recipeRepo.GetRecipeThruID(recipeID);

                if (recipe == null)
                {
                    return NotFound("The Recipe Was Not Found.");
                }

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Fetch Recipe.");
            }
        }

        [HttpPatch("{recipeID}")]
        public IActionResult UpdateRecipe(int recipeID, [FromBody] Recipe updateRecipe)
        {
            if (updateRecipe == null)
            {
                return BadRequest("Error: Recipe Data Invalid.");
            }

            try
            {
                var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                updateRecipe.UserID = userID;

                if (recipeID == null)
                    return NotFound("Recipe Can Not Be Found.");

                if (_recipeRepo.GetRecipeThruID(recipeID).UserID != userID)
                    return Forbid("Not Authorized To Update This Rating Since You Are Not The Owner.");


                _recipeRepo.UpdateRecipe(recipeID, updateRecipe);
                return StatusCode(StatusCodes.Status200OK, "Successfully Updated Recipe.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Update The Recipe.");
            }

        }

        [HttpDelete("{recipeID}")]
        public IActionResult DeleteRecipe(int recipeID)
        {
            try
            {
                var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (_recipeRepo.GetRecipeThruID(recipeID).UserID != userID)
                {
                    return Forbid("Not Authorized To Delete This Rating Since You Are Not The Owner");
                }

                _recipeRepo.DeleteRecipe(recipeID);
                return Ok("Successfully Deleted Recipe.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Delete The Recipe.");
            }
        }

        // Search recipes
        [HttpGet("Search")]
        [AllowAnonymous]
        public IActionResult SearchRecipes([FromQuery] string keyword)
        {
            try
            {
                var recipes = _recipeRepo.SearchRecipesThruTitle(keyword);

                if (recipes.Count == 0)
                {
                    return NotFound("No Recipes Was Found With The SearchWord.");
                }

                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Couldn't perform recipe search");
            }
        }
    }
}
