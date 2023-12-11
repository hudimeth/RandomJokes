using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace ReactRandomJokes.Data
{
    public class UsersRepo
    {
        private string _connectionString;
        public UsersRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using var context = new RandomJokesDbContext(_connectionString);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User LogIn(string email, string password)
        {
            var user = GetByEmail(email);
            if(user == null)
            {
                return null;
            }
            var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValidPassword)
            {
                return null;
            }
            return user;
        }

        public User GetByEmail(string email)
        {
            var context = new RandomJokesDbContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
