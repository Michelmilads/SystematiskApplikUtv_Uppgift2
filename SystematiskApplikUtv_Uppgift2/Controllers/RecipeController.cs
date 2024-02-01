using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;
using SystematiskApplikUtv_Uppgift2.Repository.Repos;
using SystematiskApplikUtv_Uppgift2.Requests;

namespace SystematiskApplikUtv_Uppgift2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepo _recipeRepo;
        private readonly ILogger<UserController> _logger;

        public RecipeController(IRecipeRepo recipeRepo, ILogger<UserController> logger)
        {
            _recipeRepo = recipeRepo;
            _logger = logger;
        }


        [HttpPost]
        public IActionResult CreateRecipe([FromQuery] NewRecipeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                Recipe newRecipe = new()
                {
                    Title = request.Title,
                    Description = request.Description,
                    Ingredients = request.Ingredients,
                    UserID = GetCurrentUser(),
                    CategoryID = request.CategoryID
                };

                _recipeRepo.CreateRecipe(newRecipe);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return Problem(ex.Message);
            }
        }

        [HttpGet]
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
                return Problem(ex.Message);
               // return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Fetch Recipe.");
            }
        }

        [HttpPatch]
        public IActionResult UpdateRecipe([FromQuery] UpdateRecipeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var recipe = _recipeRepo.GetRecipeThruID(request.RecipeID);

                if (recipe == null)
                    return NotFound("Recipe Can Not Be Found.");

                if (recipe.UserID != GetCurrentUser())
                    return Forbid("Not Authorized To Update This Recipe Since You Are Not The Owner.");

                var newRecipe = new Recipe()
                {
                    RecipeID = request.RecipeID,
                    Title = request.Title,
                    Description = request.Description,
                    Ingredients = request.Ingredients,
                    UserID = recipe.UserID,
                    CategoryID = recipe.CategoryID,
                };

                _recipeRepo.UpdateRecipe(newRecipe);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return Problem(ex.Message);
            }

        }

        [HttpDelete("{recipeID}")]
        public IActionResult DeleteRecipe(int recipeID)
        {
            try
            {
                var recipe = _recipeRepo.GetRecipeThruID(recipeID);

                if (recipe == null)
                    return NotFound();

                if (recipe.UserID != GetCurrentUser())
                {
                    return Forbid("Not Authorized To Delete This Rating Since You Are Not The Owner");
                }

                _recipeRepo.DeleteRecipe(recipeID);
                return Ok("Successfully Deleted Recipe.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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
