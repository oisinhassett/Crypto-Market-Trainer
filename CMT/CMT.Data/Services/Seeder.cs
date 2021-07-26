using System;
using System.Text;
using System.Collections.Generic;

using CMT.Core.Models;

namespace CMT.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy 
        // test data using an IUserService 
         public static void Seed(IUserService svc)
        {
            svc.Initialise();

            // add users
            svc.AddUser("Administrator", "admin@mail.com", "admin", Role.Admin);
            svc.AddUser("Manager", "manager@mail.com", "manager", Role.Manager);
            svc.AddUser("Guest", "guest@mail.com", "guest", Role.Guest);    
        }
    }
}
