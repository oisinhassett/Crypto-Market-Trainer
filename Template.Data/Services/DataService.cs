using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Template.Data.Models;
using Template.Data.Repositories;

namespace Template.Data.Services
{
    public class DataService
    {
        private DataContext db;

        // ideally use DI to inject context into service
        public DataService()
        {
            db = new DataContext();
        }

        // Used to Destroy/Rebuild the database ONLY during development
        public void Initialise()
        {
            db.Initialise();
        }

        // ======================= User management =========================

        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns>List of all users</returns>
        public IList<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        /// <summary>
        /// Return the specified user
        /// </summary>
        /// <param name="id">id of the user to retrieve</param>
        /// <returns>The user if found otherwise null</returns>
        public User GetUserById(int id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="email">EmailAddress of user to authenticate</param>
        /// <param name="password">Plain text password of user to authenticate</param>
        /// <returns>The user if authenticated, otherewise null</returns>
        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            return db.Users
                .Where(u => u.EmailAddress == email && password == u.Password)
                .FirstOrDefault();
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="u">User to register</param>
        /// <returns>The user if registered, otherewise null</returns>
        public User RegisterUser(User u)
        {
            // check that the user does not already exist (unique user name)
            var exists = db.Users.FirstOrDefault(x => x.EmailAddress == u.EmailAddress);
            if (exists != null)
            {
                return null;
            }

            // should really encrypt the password before storing in database          
            db.Users.Add(u);
            db.SaveChanges();
            return u;
        }

        /// <summary>
        /// Find a user by EmailAddress (name should be unique)
        /// </summary>
        /// <param name="email">user EmailAddress</param>
        /// <returns>The user if found, otherewise null</returns>
        public User GetUserByEmailAddress(string email)
        {
            return db.Users.FirstOrDefault(u => u.EmailAddress == email);
        }

    }
}
