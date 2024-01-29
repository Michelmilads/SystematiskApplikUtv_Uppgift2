using SystematiskApplikUtv_Uppgift2.Entities;

namespace SystematiskApplikUtv_Uppgift2.Repository.Interfaces
{
    public interface IRatingRepo
    {
        void CreateRating(Ratings ratings);
        void UpdateRatings(int ratingID, Ratings updateRating);
        void DeleteRating(int ratingID);
        Ratings GetRatingThruRatingID(int ratingID);
        Ratings GetRatingThruUserID(int userID);
        List<Ratings> GetRatingsThruRecipeID(int recipeID);
        List<Ratings> CheckIfUserIsOwner(int recipeID, int userID);
    }
}
