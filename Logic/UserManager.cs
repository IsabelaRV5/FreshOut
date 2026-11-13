using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Data.Repositories.Interfaces;
using Data.Entities;

namespace Logic
{
    /// <summary>
    /// this class is the logic for User Authentication
    /// It handles the validation of user credentials and for account creation
    /// </summary>
    /// <remarks>
    /// author: Xena Tat
    /// date: 17.04.2026
    /// </remarks>
    public class UserManager
    {
        private readonly IUserRepository _repo;
        
        public UserManager(IUserRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// <c>ValidateCredentials</c>
        /// Validates credentials of user by comparing hashed inputs to database
        /// created hash.
        /// 
        /// </summary>
        /// <param name="username"></param> User's input username
        /// <param name="password"></param> User's input password (plain text)
        /// <returns> Returns User to cache in App</returns>
        public User ValidateCredentials(string username, string password)
        {
            // Finds user by username in Database
            User findUser = _repo.GetUserByUsername(username);

            // Hashed input credentials
            byte[] hashedPw = SHA256.HashData(Encoding.UTF8.GetBytes(username + password));

            // Checks if user is found in database and if hash matches
            if (findUser != null && findUser.PasswordHash.Equals(Convert.ToHexString(hashedPw)))
            {
                return findUser;
            }
            return null;
        }
        
        /// <summary>
        /// <c>RegisterUser</c>
        /// 
        /// Checks database if username is taken, if not register new account
        /// Method of hashing: SHA256 -> Username + Password
        /// 
        /// </summary>
        /// <param name="username"></param> User's chosen username to log in
        /// <param name="password"></param> User's plain text password
        /// <param name="iconPath"></param> User's chosen Icon
        /// <returns> True if user is successfully added to db, False if user is invalid </returns>
        public Boolean RegisterUser(string username, string password, string iconPath)
        {
            // Checks if inputs are valid - Trims string and check length if > 0
            if (!(username.Trim().Length > 0 && password.Trim().Length > 0 && iconPath.Trim().Length > 0))
            {
                return false;
            }

            // Validate username
            User findUser = _repo.GetUserByUsername(username);
            if(findUser == null)
            {
                // Get username + password in bytes array
                // Converting string -> bytes because SHA256 only takes bytes[] as input
                byte[] userPassHash = Encoding.UTF8.GetBytes(username + password);
                
                // Hash username + password and convert back to string
                string hashed = Convert.ToHexString(SHA256.HashData(userPassHash));

                // Create new user
                User newUser = new User() { Username = username, PasswordHash = hashed, IconPath = iconPath };

                // Add user to database
                _repo.AddUser(newUser);
                return true;
            }
            return false;
        }
    }
}
