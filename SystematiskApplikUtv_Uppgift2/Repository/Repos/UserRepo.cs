using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class UserRepo : IUserRepo
    {
        public void CreateUser(User user)
        {
            using SqlConnection db = new(DatabaseConnection.connString);

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", user.UserName);
            parameters.Add("@PassWord", user.PassWord);
            parameters.Add("@Email", user.Email);

            db.Execute("CreateUser", parameters, commandType: CommandType.StoredProcedure);
        }

        public void UpdateUser(int userID, string passWord)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);
                parameters.Add("@PassWord", passWord);

                db.Execute("UpdateUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteUser(int userID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);

                db.Execute("DeleteUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public User AuthenticateUser(string userName, string passWord)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                parameters.Add("@PassWord", passWord);

                return db.QueryFirstOrDefault<User>("Login", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public User GetUserThruID(int userID)
        {
            using (SqlConnection db = new(DatabaseConnection.connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);

                return db.QuerySingleOrDefault<User>("GetUserThruID", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
