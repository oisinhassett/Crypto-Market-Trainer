using System;
using Xunit;
using Microsoft.AspNetCore.Identity;
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

        public void TestHash()
        {
		    var ph = new Microsoft.AspNetCore.Identity.PasswordHasher();
		
		    var hash = ph.HashPassword("test");
		
		    var isCurrentHashValid = ph.VerifyHashedPassword(hash, "test");
		    var isOlderHashValid = ph.VerifyHashedPassword("AO7kszlVq1gUsEN6eEwH9WcbppmJlG0qtZpmG65xdklCa89AalTbiA+uXXCOVjzDXw==", "test");

            Assert.True(isCurrentHashValid);

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
