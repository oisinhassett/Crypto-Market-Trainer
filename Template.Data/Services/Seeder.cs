using System;
using System.Text;
using System.Collections.Generic;

using Template.Core.Models;

namespace Template.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy 
        // test data using the DataService 
        public static void Seed()
        {
            var service = new DataService();
            service.Initialise();
            
            service.RegisterUser(new User { 
                Name = "admin", 
                EmailAddress="admin@mail.com", 
                Password = "admin", 
                Role = Role.Admin 
            });

            service.RegisterUser(new User { 
                Name = "guest", 
                EmailAddress="guest@mail.com", 
                Password = "guest", 
                Role = Role.Guest 
            });
        }
    }
}
