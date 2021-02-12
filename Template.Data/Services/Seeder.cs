using System;
using System.Text;
using System.Collections.Generic;

using Template.Core.Models;

namespace Template.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy 
        // test data using an IUserService 
         public static void Seed(IUserService svc)
        {
            svc.Initialise();

            // add users
            svc.AddUser("admin", "admin@mail.com", "admin", Role.Admin);
            svc.AddUser("guest", "guest@mail.com", "guest", Role.Guest);    
        }
    }
}
