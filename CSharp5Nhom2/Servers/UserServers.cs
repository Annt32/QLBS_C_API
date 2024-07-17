using CSharp5Nhom2.Iservers;
using CSharp5Nhom2.Models;

namespace CSharp5Nhom2.Servers
{
    public class UserServers : IUserServers
    {
        HttpClient _Client;
        public UserServers()
        {
            _Client = new HttpClient();
        }

        public bool CrealUser(User user)
        {
            string reposURL = "https://localhost:7249/api/User/create_user";
            var repose = _Client.PostAsJsonAsync(reposURL, user);
            if (repose.IsCompletedSuccessfully)
            {
                return true;
            }
            return false;
        }

        public bool DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(string id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUsery(User user)
        {
            throw new NotImplementedException();
        }
    }
}
