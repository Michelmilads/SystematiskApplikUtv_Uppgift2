using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class RatingRepo : IRatingRepo
    {
        public void CreateRating(Ratings ratings)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", ratings.RecipeID);
                parameters.Add("@RatedByUserID", ratings.RatedByUserID);
                parameters.Add("@RatingValue", ratings.RatingValue);

                db.Execute("RateRecipe", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Ratings> GetRatingsThruRecipeID(int recipeID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);

                return db.Query<Ratings>("GetRatingsThruRecipeID", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Ratings GetRatingThruRatingID(int ratingID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RatingID", ratingID);

                return db.QueryFirstOrDefault<Ratings>("GetRatingThruRatingID", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public List<Ratings> GetRatingThruUserID(int userID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RatedByUserID", userID);

                return db.Query<Ratings>("GetRatingThruUserID", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void DeleteRating(int ratingID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RatingID", ratingID);

                db.Execute("DeleteRating", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
