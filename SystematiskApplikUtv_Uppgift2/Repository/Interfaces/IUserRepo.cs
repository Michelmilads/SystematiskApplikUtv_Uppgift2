using SystematiskApplikUtv_Uppgift2.Entities;

namespace SystematiskApplikUtv_Uppgift2.Repository.Interfaces
{
    public interface IUserRepo
    {
        void CreateUser(User user);
        void UpdateUser(int userID, string passWord);
        void DeleteUser(int id);
        User AuthenticateUser(string userName, string passWord);
        User GetUserThruID(int userID);
    }
}

