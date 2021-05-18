using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Template.Core.Models;
using Template.Data.Services;
using Template.Web.ViewModels;

namespace Template.Web.Controllers
{

    [ApiController]    
    [Route("api")]     
    // set default auth scheme as we are using both cookie and jwt authentication
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiController : ControllerBase
    {
        private readonly IUserService svc;       
        private readonly IConfiguration config; // jwt settings
      
        public ApiController(IUserService service, IConfiguration configuration)
        {      
            config = configuration;            
            svc = service;
        }

        // POST api/user/login
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<User> Login(UserLoginViewModel login)        
        {                     
            var user = svc.Authenticate(login.Email, login.Password);            
            if (user == null)
            {
                return BadRequest(new { message = "Email or Password are incorrect" });
            }
            // sign jwt token to use in secure api requests
            var authUser = SignInJwt(user);

            return Ok(authUser);
        }  

        // Sign user in using JWT authentication
        private UserAuthenticatedViewModel SignInJwt(User user)
        {
            return new UserAuthenticatedViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,              
                Role = user.Role,
                 Token = AuthBuilder.BuildJwtToken(user, config),
            };
        }     

    }
}