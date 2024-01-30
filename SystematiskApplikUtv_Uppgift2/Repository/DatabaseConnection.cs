using Microsoft.Data.SqlClient;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;
using static SystematiskApplikUtv_Uppgift2.Repository.DatabaseConnection;

namespace SystematiskApplikUtv_Uppgift2.Repository
{
    public class DatabaseConnection : IDatabaseConnection
    {
        
        private readonly string? _connString;

        public DatabaseConnection(IConfiguration config)
        {
            _connString = config.GetConnectionString("FoodRecipes");
        }

        public SqlConnection GetConnection()
        {
           return new SqlConnection(_connString);
        }   
    }
}
