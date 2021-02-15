using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Template.Core.Models;

namespace Template.Web.Helpers
{
    // Static class providing Authentication Helpers
    // Configured to provide CookieBased Authentication Scheme
    public static class AuthHelper
    {
        public static string AuthenticationScheme => CookieAuthenticationDefaults.AuthenticationScheme;

        // Configures Cookie Based Authentication and enables the Authorize Tag helper
        public static void AddAuthSimple(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.AccessDeniedPath = "/User/ErrorNotAuthorised";
                        options.LoginPath = "/User/ErrorNotAuthenticated";
                    });

            // AMC - Required if using the AuthorizeTagHelper 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This private method is used to generate a ClaimsPrincipal used when signing in
        // Claims can be added to customise the ClaimsIdentity used in generating the ClaimsPrincipal. 
        // User Id identity is configured via Sid claim type 
        // Multiple roles can be added where user model is configured for multiple roles
        public static ClaimsPrincipal BuildPrincipal(User u)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, u.Id.ToString()),
                new Claim(ClaimTypes.Name, u.Name),
                new Claim(ClaimTypes.Role, u.Role.ToString()),
                //new Claim(ClaimTypes.Role, Role.Guest.ToString()),
            }, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                return principal;
        }
    }

}