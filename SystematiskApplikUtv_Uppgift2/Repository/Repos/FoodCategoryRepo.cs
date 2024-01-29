using Dapper;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class FoodCategoryRepo : IFoodCategoryRepo
    {
        private readonly IDatabaseConnection _connString;

        public FoodCategoryRepo(IDatabaseConnection connString)
        {
            _connString = connString;
        }

        public List<FoodCategory> GetAllFoodCategories()
        {
            using (var db = _connString.GetConnection())
            {
                return db.Query<FoodCategory>("GetAllFoodCategories", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
