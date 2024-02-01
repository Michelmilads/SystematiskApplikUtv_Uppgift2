namespace SystematiskApplikUtv_Uppgift2.Requests
{
    public class NewRecipeRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Ingredients { get; set; }
        public int CategoryID { get; set; }
    }
}
