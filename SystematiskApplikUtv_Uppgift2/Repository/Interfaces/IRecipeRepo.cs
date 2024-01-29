using SystematiskApplikUtv_Uppgift2.Entities;

namespace SystematiskApplikUtv_Uppgift2.Repository.Interfaces
{
    public interface IRecipeRepo
    {  
        void CreateRecipe(Recipe recipe);
        void UpdateRecipe(int recipeID, Recipe updateRecipe);
        void DeleteRecipe(int recipeID);
        Recipe GetRecipeThruID(int recipeID);
        List<Recipe> SearchRecipesThruTitle(string searchWord); // string title
    }
}
