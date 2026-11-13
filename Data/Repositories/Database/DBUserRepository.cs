using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.Repositories.Database
{
    /// <summary>
    /// DB implementation of IUserRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-21  
    /// </remarks>
    public class DBUserRepository : IUserRepository
    {
        private FreshOutContext _context;

        /// <summary>
        /// Constructor that initializes the DB repository
        /// </summary>
        public DBUserRepository(FreshOutContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all registered users</returns>
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
        /// <summary>
        /// Gets a user by their id
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user if found or null if not found</returns>
        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }
        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>The matching user if found or null if not found</returns>
        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username.Equals(username));
        }
        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user">The to add</param>
        public void AddUser(User user)
        {
            if (user != null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
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
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
