using Data.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Mock
{
    /// <summary>
    /// Mock implementation of IUserRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-19  
    /// </remarks>
    public class MockUserRepository : IUserRepository
    {

        private List<User> _users;
        private int _nextId;

        /// <summary>
        /// Constructor that initializes the mock repository
        /// </summary>
        public MockUserRepository()
        {
            // Info for user 1
            string username1 = "Alice";
            string pass1 = "alice123";
            string hashed1 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(username1 + pass1)));

            // Info for user 2
            string username2 = "Clara";
            string pass2 = "clara123";
            string hashed2 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(username2 + pass2)));

            _users = new List<User>
            {
                 new User
                {
                    UserId = 1,
                    Username = username1,
                    PasswordHash = hashed1,
                    IconPath = ""

                },
                new User
                {
                    UserId = 2,
                    Username = username2,
                    PasswordHash = hashed2,
                    IconPath = ""
                }
            };

            _nextId = _users.Max(u => u.UserId) + 1;
        }
        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all registered users</returns>
        public List<User> GetUsers()
        { 
            return _users;
        }
        /// <summary>
        /// Gets a user by their id
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user if found or null if not found</returns>
        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.UserId == id);
        }
        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>The matching user if found or null if not found</returns>
        public User? GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username));
        }
        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user">The to add</param>
        public void AddUser(User user)
        {
            if (user != null)
            {
                user.UserId = _nextId;
                _users.Add(user);
            }
        }
        /// <summary>
        /// Removes a user by their id
        /// </summary>
        /// <param name="id">The id of the user to remove.</param>
        public void RemoveUser(int id)
        {
             User? user = GetUserById(id);

            if (user != null)
            {
                _users.Remove(user);
            }
        }
    }
}
