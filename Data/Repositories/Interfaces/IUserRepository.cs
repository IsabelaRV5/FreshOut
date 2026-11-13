using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository responsible for managing User CRUD operations.
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-18  
    /// </remarks>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all registered users</returns>
        public List<User> GetUsers();
        /// <summary>
        /// Gets a user by their id
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user if found or null if not found</returns>
        public User? GetUserById(int id);
        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>The matching user if found or null if not found</returns>
        public User? GetUserByUsername(string username);
        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user">The to add</param>
        public void AddUser(User user);
        /// <summary>
        /// Removes a user by their id
        /// </summary>
        /// <param name="id">The id of the user to remove.</param>
        public void RemoveUser(int id);

    }
}
