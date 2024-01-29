using Dapper;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class RecipeRepo : IRecipeRepo
    {
        private readonly IDatabaseConnection _connString;

        public RecipeRepo(IDatabaseConnection connString)
        {
            _connString = connString;
        }


        public Recipe GetRecipeThruID(int recipeID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);

                return db.QueryFirstOrDefault<Recipe>("GetRecipeById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void CreateRecipe(Recipe recipe)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Title", recipe.Title);
                parameters.Add("@Description", recipe.Description);
                parameters.Add("@Ingredients", recipe.Ingredients);
                parameters.Add("@UserID", recipe.UserID);
                parameters.Add("@CategoryID", recipe.CategoryID);

                db.Execute("CreateRecipe", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateRecipe(int recipeID, Recipe updateRecipe)
        {
            // Check authorization here.

            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);
                parameters.Add("@Title", updateRecipe.Title);
                parameters.Add("@Description", updateRecipe.Description);
                parameters.Add("@Ingredients", updateRecipe.Ingredients); // ändra category kan vara bra att lägga till
                parameters.Add("@CategoryID", updateRecipe.CategoryID);

                // debugging
                Console.WriteLine($"Recipe ID: {recipeID}");
                Console.WriteLine($"Title: {updateRecipe.Title}");
                Console.WriteLine($"Description: {updateRecipe.Description}");
                Console.WriteLine($"Ingredients: {updateRecipe.Ingredients}");
                Console.WriteLine($"CategoryID: {updateRecipe.CategoryID}");

                db.Execute("UpdateRecipe", parameters, commandType: CommandType.StoredProcedure);
            }


        }

        public void DeleteRecipe(int recipeID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);

                db.Execute("DeleteRecipe", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Recipe> SearchRecipesThruTitle(string searchKeyWord)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SearchCondition", searchKeyWord);

                return db.Query<Recipe>("SearchRecipes", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
