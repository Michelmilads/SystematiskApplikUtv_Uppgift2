using Microsoft.Data.SqlClient;

namespace SystematiskApplikUtv_Uppgift2.Repository.Interfaces
{
    public interface IDatabaseConnection
    {
        SqlConnection GetConnection();
    }
}
