using System.Collections.Generic;
using System.Linq;

namespace IgnacioBackendApp.Services
{
    public class UserService
    {
        private readonly List<User> _users = new();

        public bool UserExists(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        public void CreateUser(string username, string passwordHash, string email)
        {
            _users.Add(new User { Username = username, PasswordHash = passwordHash, Email = email });
        }

        public User? GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }

    public class User
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Email { get; set; }
    }
}
