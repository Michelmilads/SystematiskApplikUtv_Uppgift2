namespace SystematiskApplikUtv_Uppgift2.Entities
{
    public class Ratings
    {
        public int RatingID { get; set; }
        public int RatingValue { get; set; }
        public int RecipeID { get; set; }
        public int RatedByUserID { get; set; }
    }
}
