using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class FoodCategoryRepo : IFoodCategoryRepo
    {
        public List<FoodCategory> GetAllFoodCategories()
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                return db.Query<FoodCategory>("GetAllFoodCategories", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
