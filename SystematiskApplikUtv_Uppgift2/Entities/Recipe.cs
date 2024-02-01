namespace SystematiskApplikUtv_Uppgift2.Entities
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string? Title { get; set;}
        public string? Description { get; set;}
        public string? Ingredients { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
    }
}
