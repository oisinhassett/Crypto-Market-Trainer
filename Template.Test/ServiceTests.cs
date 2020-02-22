using System;
using Xunit;

using Template.Data.Models;
using Template.Data.Services;

namespace Template.Test
{
    public class ServiceTests
    {
        private DataService service;

        public ServiceTests()
        {
            service = new DataService();
            service.Initialise();
        }

        [Fact]
        public void EmptyDbShouldReturnNoUsers()
        {
            var users = service.GetAllUsers();

            Assert.Equal(0, users.Count);
        }
        
        [Fact]
        public void AddingUsersShouldWork()
        {
            var u1 = new User { Name = "admin", EmailAddress = "admin@mail.com", Password = "admin", Role = Role.Admin };
            var u2 = new User { Name = "guest", EmailAddress = "guest@mail.com", Password = "guest", Role = Role.Guest };
            service.RegisterUser(u1);
            service.RegisterUser(u2);

            var users = service.GetAllUsers();

            Assert.Equal(2, users.Count);
        }

    }
}
