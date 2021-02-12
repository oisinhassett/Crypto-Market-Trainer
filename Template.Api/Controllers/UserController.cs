using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Template.Core.Models;
using Template.Data.Services;
using Template.Api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Template.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUserService _service;

        // Inject Services
        public UserController(IUserService service, IConfiguration configuration)
        {
            _configuration = configuration;
            _service = service;       
        }

        [HttpGet]
        [Authorize]
        public IActionResult UserList()
        {
            var users = _service.GetUsers(); 
            return Ok(users);
        } 

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var user = _service.Authenticate(login.Email, login.Password);            
            if (user == null)
            {          
                return Unauthorized( new ErrorResponse { Message = "Username and/or password are invalid."} );
            }
            user.Password = "";
            user.Token = JwtHelper.SignJwtToken(user, _configuration);
            return Ok( user );
        }
        
        [HttpPost("register")]
        public IActionResult Post([FromBody]User model)
        {
            var user = _service.AddUser(model.Name,model.Email,model.Password, model.Role);       
            if (user == null)
            {  
                return BadRequest(new ErrorResponse { Message = "Error creating user" } );
            }
            user.Token = JwtHelper.SignJwtToken(user, _configuration);
            return CreatedAtAction(nameof(Login), user);
        }

        [HttpPut("update")]
        public IActionResult Put([FromBody]User model)
        {
            var user = _service.UpdateUser(model);       
            if (user == null)
            {  
                return BadRequest(new ErrorResponse { Message = "Error updating user" } );
            }
            return Ok(user);
        }  

        [HttpGet("verify/{email}/{id?}")]
        public IActionResult VerifyEmailAvailable(string email, int? id)
        {
            return Ok(_service.GetUserByEmail(email, id)==null);
        }
    }
}