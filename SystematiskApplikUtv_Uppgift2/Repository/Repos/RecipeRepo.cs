using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class RecipeRepo : IRecipeRepo
    { 
        public Recipe GetRecipeThruID(int recipeID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);

                return db.QueryFirstOrDefault<Recipe>("GetRecipeThruID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void CreateRecipe(Recipe recipe)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
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

        public void UpdateRecipe(Recipe recipe)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipe);
                parameters.Add("@Title", recipe.Title);
                parameters.Add("@Description", recipe.Description);
                parameters.Add("@Ingredients", recipe.Ingredients);
                parameters.Add("@CategoryID", recipe.CategoryID);                                                                                                                                               

                db.Execute("UpdateRecipe", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public void DeleteRecipe(int recipeID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);

                db.Execute("DeleteRecipe", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Recipe> SearchRecipesThruTitle(string searchKeyWord)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SearchCondition", searchKeyWord);

                return db.Query<Recipe>("SearchRecipes", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
