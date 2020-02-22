using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using Template.Data.Services;
using Template.Data.Models;
using Template.Web.ViewModels;

/**
 *  AMC - User Management Controller providing registration
 *        and login functionality
 */
namespace Template.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly DataService _svc;

        public UserController()
        {
            // ideally we should use Dependency Injection
            _svc = new DataService();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("EmailAddress,Password")] User m)
        {
            var user = _svc.Authenticate(m.EmailAddress, m.Password);
            if (user == null)
            {
                ModelState.AddModelError("EmailAddress", "Invalid Login Credentials");
                ModelState.AddModelError("Password", "Invalid Login Credentials");
                return View(m);
            }

            // Log user in, using cookie authentication
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                BuildPrincipal(user)
            );

            Alert("Successfully Logged in", AlertType.info);

            return Redirect("/");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Name,EmailAddress,Password,PasswordConfirm,Role")] RegisterViewModel m)       
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            var user = _svc.RegisterUser(new User
            {
                Name = m.Name,
                EmailAddress = m.EmailAddress,
                Password = m.Password,
                Role = m.Role
            });
            if (user == null) {
                Alert("There was a problem Registering. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Registered. Now login", AlertType.info);

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ErrorNotAuthorised() => View();
        public IActionResult ErrorNotAuthenticated() => View();


        // Called by Remote Validation attribute on RegisterViewModel to verify email address is unique
        [AcceptVerbs("GET", "POST")]
        public IActionResult GetUserByEmailAddress(string EmailAddress)
        {
            var user = _svc.GetUserByEmailAddress(EmailAddress);
            if (user != null)
            {
                return Json($"A user with this email address {EmailAddress} already exists.");
            }
            return Json(true);                  
        }


        // This private method is used to generate a ClaimsPrincipal used when signing in
        // Claims can be added to customise the ClaimsIdentity used in generating the ClaimsPrincipal. 
        // For example, new Claim("Id", u.Id.ToString(), ClaimValueTypes.String) // adds user id to identity 
        // Also, multiple roles can be added where a user is configured for multiple roles
        private ClaimsPrincipal BuildPrincipal(User u)
        {
            var identity = new ClaimsIdentity(new[]
            {  
                new Claim(ClaimTypes.Name, u.Name),
                new Claim(ClaimTypes.Role, u.Role.ToString()),          
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}