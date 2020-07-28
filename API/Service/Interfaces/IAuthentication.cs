using Service.Models;

namespace Service.Interfaces
{
    public interface IAuthentication
    {
        UserModel Authenticate(string email, string password);
        void Add(UserModel user);
    }
}
