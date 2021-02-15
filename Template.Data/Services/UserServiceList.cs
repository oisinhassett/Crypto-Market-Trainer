using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

using Template.Core.Models;
using Template.Core.Security;
using System.IO;

namespace Template.Data.Services
{
   public class UserServiceList : IUserService
    {
        private static string STORE = "users.json";
        private IList<User> Users;

        public UserServiceList()
        {           
            Load();
        }

        // load data from local json store
        private void Load()
        {
            try {
                string json = File.ReadAllText(STORE);            
                Users = JsonSerializer.Deserialize<List<User>>(json);
            } 
            catch (Exception )
            {
                Users = new List<User>();
            }
        }

        // write to local json store
        private void Store()
        {
            var json = JsonSerializer.Serialize(Users);
            File.WriteAllText(STORE, json);
        }

        public void Initialise()
        {
            Users.Clear(); // wipe all Users
        }

        // ------------------ User Related Operations ------------------------

        // retrieve list of Users
        public IList<User> GetUsers()
        {
            return Users;
        }

        // Retrive User by Id 
        public User GetUser(int id)
        {
            return Users.FirstOrDefault(s => s.Id == id);
        }

        // Add a new User checking a User with same email does not exist
        public User AddUser(string name, string email, string password, Role role)
        {     
            var existing = GetUserByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var s = new User
            {
                // simple mechanism to calculate next Id
                Id = Users.Count + 1,
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password), // can hash if required  
                Role = role             
            };
            Users.Add(s);
            Store(); // write to local json store
            return s; // return newly added User
        }

        // Delete the User identified by Id returning true if deleted and false if not found
        public bool DeleteUser(int id)
        {
            var s = GetUser(id);
            if (s == null)
            {
                return false;
            }
            Users.Remove(s);
            Store(); // write to local json store
            return true;
        }

        // Update the User with the details in updated 
        public User UpdateUser(User updated)
        {
            // verify the User exists
            var User = GetUser(updated.Id);
            if (User == null)
            {
                return null;
            }
            // update the details of the User retrieved and save
            User.Name = updated.Name;
            User.Email = updated.Email;
            User.Password = Hasher.CalculateHash(updated.Password);
            User.Role = updated.Role;   
            Store();   // write to local json file     
            return User;
        }

         public User GetUserByEmail(string email, int? id=null)
        {
            return Users.FirstOrDefault(u => u.Email == email && ( id == null || u.Id != id));
        }

        public IList<User> GetUsersQuery(Func<User,bool> q)
        {
            return Users.Where(q).ToList();
        }

        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
            //return (user != null && user.Password == password ) ? user: null;
        }
   
    }
}