using SystematiskApplikUtv_Uppgift2.Entities;

namespace SystematiskApplikUtv_Uppgift2.Repository.Interfaces
{
    public interface IRatingRepo
    {
        void CreateRating(Ratings ratings);
        void DeleteRating(int ratingID);
        Ratings GetRatingThruRatingID(int ratingID);
        List<Ratings> GetRatingThruUserID(int userID);
        List<Ratings> GetRatingsThruRecipeID(int recipeID);
    }
}
