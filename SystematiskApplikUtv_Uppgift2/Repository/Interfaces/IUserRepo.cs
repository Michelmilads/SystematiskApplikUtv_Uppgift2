using SystematiskApplikUtv_Uppgift2.Entities;

namespace SystematiskApplikUtv_Uppgift2.Repository.Interfaces
{
    public interface IUserRepo
    {
        void CreateUser(User user);
        void UpdateUser(int userID, User updateUser);
        void DeleteUser(int id);
        User AuthenticateUser(User user);
        User GetUserThruID(int userID);
    }
}

