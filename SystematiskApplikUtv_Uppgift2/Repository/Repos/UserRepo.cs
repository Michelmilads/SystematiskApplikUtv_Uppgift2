using Dapper;
using System.Data;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Repository.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly IDatabaseConnection _connString;

        public UserRepo(IDatabaseConnection connString)
        {
            _connString = connString;
        }

        public void CreateUser(User user)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@PassWord", user.PassWord);
                parameters.Add("@Email", user.Email);

                db.Execute("CreateUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateUser(int userID, User updateUser)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);
                parameters.Add("@UserName", updateUser.UserName);
                parameters.Add("@PassWord", updateUser.PassWord);
                parameters.Add("@Email", updateUser.Email);

                db.Execute("UpdateUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteUser(int userID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);

                db.Execute("DeleteUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public User AuthenticateUser(User user)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@PassWord", user.PassWord);

                return db.QuerySingleOrDefault<User>("Login", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public User GetUserThruID(int userID)
        {
            using (var db = _connString.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);

                return db.QuerySingleOrDefault<User>("GetUserThruID", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
