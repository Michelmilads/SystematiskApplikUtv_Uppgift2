using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodCategoryController : ControllerBase
    {
        private readonly IFoodCategoryRepo _foodCategoryRepo;

        public FoodCategoryController(IFoodCategoryRepo foodCategoryRepo)
        {
            _foodCategoryRepo = foodCategoryRepo;
        }

        [HttpGet]
        public IActionResult GetAllFoodCategories()
        {
            try
            {
                var categories = _foodCategoryRepo.GetAllFoodCategories();

                if (categories.Count == 0)
                {
                    return NotFound("Error: No Food Categories Could Be Found");
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Food Categories Could Not Be Fetched.");
            }
        }
    }
}
