namespace SystematiskApplikUtv_Uppgift2.Requests
{
    public class UpdateRecipeRequest
    {
        public int RecipeID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Ingredients { get; set; }
    }
}
