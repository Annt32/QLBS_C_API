using CSharp5Nhom2.Models;

namespace CSharp5Nhom2.Iservers
{
    public interface IUserServers
    {
        List<User> GetAll();
        User GetById(string id);
        bool CrealUser(User user);
        bool UpdateUsery(User user);
        bool DeleteUser(string id);
    }
}
