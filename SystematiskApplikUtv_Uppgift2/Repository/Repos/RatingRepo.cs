using Dapper;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class RatingRepo : IRatingRepo
    {
        private readonly IDatabaseConnection _connString;

        public RatingRepo(IDatabaseConnection connString)
        {
            _connString = connString;
        }

        public void CreateRating(Ratings ratings)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", ratings.RecipeID);
                parameters.Add("@RatedByUserID", ratings.RatedByUserID);
                parameters.Add("@RatingValue", ratings.RatingValue);

                db.Execute("RateRecipe", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        
        public List<Ratings> CheckIfUserIsOwner(int recipeID, int userID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);
                parameters.Add("@UserID", userID);

                var result = db.Query<Ratings, Recipe, Ratings>(
                    "CheckIfUserIsOwner",
                    (ratings, recipes) =>
                    {
                        ratings.Recipe = recipes;
                        return ratings;
                    },
                    parameters,
                    splitOn: "RecipeID",
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return result;
            }
        }

        public List<Ratings> GetRatingsThruRecipeID(int recipeID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RecipeID", recipeID);

                return db.Query<Ratings>("GetRatingsThruRecipeID", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Ratings GetRatingThruRatingID(int ratingID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RatingID", ratingID);

                return db.QueryFirstOrDefault<Ratings>("GetRatingsThruRatingID", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public Ratings GetRatingThruUserID(int userID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RatedByUserID", userID);

                return db.QueryFirstOrDefault<Ratings>("GetRatingThruUserID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateRatings(int ratingID, Ratings updateRating)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RatingID", ratingID);
                parameters.Add("@RatingValue", updateRating.RatingValue);

                db.Execute("UpdateRating", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteRating(int ratingID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RatingID", ratingID);

                db.Execute("DeleteRating", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
