using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using Template.Data.Services;
using Template.Core.Models;
using Template.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Template.Web.Helpers;

/**
 *  AMC - User Management Controller providing registration
 *        and login functionality
 */
namespace Template.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _svc;

        public UserController(IUserService svc)
        {            
            _svc = svc;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] User m)
        {
            var user = _svc.Authenticate(m.Email, m.Password);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid Login Credentials");
                ModelState.AddModelError("Password", "Invalid Login Credentials");
                return View(m);
            }

            // Log user in, using cookie authentication
            await SignIn(user);

            Alert("Successfully Logged in", AlertType.info);

            return Redirect("/");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Name,Email,Password,PasswordConfirm,Role")] UserRegisterViewModel m)       
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            var user = _svc.AddUser(m.Name, m.Email,m.Password, m.Role);
            if (user == null) {
                Alert("There was a problem Registering. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Registered. Now login", AlertType.info);

            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult Manage()
        {
            var user = _svc.GetUser((this.Identity()).Value);
            var userViewModel = new UserManageViewModel { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email,                 
                Role = user.Role
            };
            return View(userViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage([Bind("Id,Name,Email,Role")] UserManageViewModel m)       
        {
            var user = _svc.GetUser(m.Id);
            
            if (!ModelState.IsValid || user == null)
            {
                return View(m);
            } 
            // update user details 
            user.Name = m.Name;
            user.Email = m.Email;
            user.Role = m.Role;
        
            var updated = _svc.UpdateUser(user);
            if (updated == null) {
                Alert("There was a problem Updating. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Updated Account Details", AlertType.info);
            
            // sign the user in with updated details)
            await SignIn(user);

            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public IActionResult Password()
        {
            var user = _svc.GetUser((this.Identity()).Value);
            var passwordViewModel = new UserPasswordViewModel { 
                Id = user.Id, 
                Password = user.Password, 
                PasswordConfirm = user.Password, 
            };
            return View(passwordViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password([Bind("Id,Password,PasswordConfirm")] UserPasswordViewModel m)       
        {
            var user = _svc.GetUser(m.Id);
            
            if (!ModelState.IsValid || user == null)
            {
                return View(m);
            }  
            // update the password
            user.Password = m.Password; 
            // save changes      
            var updated = _svc.UpdateUser(user);
            if (updated == null) {
                Alert("There was a problem Updating the password. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Updated Password", AlertType.info);
            // sign the user in with updated details)
            await SignIn(user);

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        // Return not authorised and not authenticated views
        public IActionResult ErrorNotAuthorised() => View();
        public IActionResult ErrorNotAuthenticated() => View();


        // Called by Remote Validation attribute on RegisterViewModel to verify email address is unique
        [AcceptVerbs("GET", "POST")]
        public IActionResult GetUserByEmailAddress(string email)
        {
            // get identity id of signed in user
            var id = this.Identity();
            // check if email is available, unless already owned by user with id
            var user = _svc.GetUserByEmail(email, id);
            if (user != null)
            {
                return Json($"A user with this email address {email} already exists.");
            }
            return Json(true);                  
        }


        // Sign user in using authentication scheme configured via AuthHelper
        private async Task SignIn(User user)
        {
            await HttpContext.SignInAsync(
                AuthHelper.AuthenticationScheme,
                AuthHelper.BuildPrincipal(user)
            );
        }
    }
}