using Microsoft.Data.SqlClient;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;
using static SystematiskApplikUtv_Uppgift2.Repository.DatabaseConnection;

namespace SystematiskApplikUtv_Uppgift2.Repository
{
    public class DatabaseConnection
    {
        public static readonly string connString = "Data Source=Michel;Initial Catalog=FoodRecipes;Integrated Security=true;trustservercertificate=true";
    }
}
