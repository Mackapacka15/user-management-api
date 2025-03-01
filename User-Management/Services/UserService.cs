using UserManagement.Exceptions;

namespace UserManagement.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUser(string name);
        User CreateUser(User user);
        void UpdateUser(string name, User user);
        void DeleteUser(string name);
    }

    public class UserService : IUserService
    {
        private readonly Dictionary<string, User> _users = new Dictionary<string, User>
        {
            {
                "Alice",
                new User { Name = "Alice", Age = 30 }
            },
            {
                "Bob",
                new User { Name = "Bob", Age = 25 }
            },
            {
                "Charlie",
                new User { Name = "Charlie", Age = 40 }
            },
        };

        public IEnumerable<User> GetUsers()
        {
            return _users.Values;
        }

        public User GetUser(string name)
        {
            if (!_users.TryGetValue(name, out var user))
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }

        public User CreateUser(User user)
        {
            if (_users.ContainsKey(user.Name))
            {
                throw new BadRequestException("User already exists");
            }
            _users[user.Name] = user;
            return user;
        }

        public void UpdateUser(string name, User user)
        {
            if (!_users.ContainsKey(name))
            {
                throw new NotFoundException("User not found");
            }
            _users[name] = user;
        }

        public void DeleteUser(string name)
        {
            if (!_users.Remove(name))
            {
                throw new NotFoundException("User not found");
            }
        }
    }
}
