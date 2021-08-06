using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ToDo.App.Pages.Account
{
    public class Logout : PageModel
    {
        private readonly IConfigurationSection _configuration;

        public Logout(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("Auth");
        }
        
        public IActionResult OnGet()
        {
            var redirectUri = _configuration["PostLogoutRedirectUri"] ?? "/";
            var options = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            }; 
            return SignOut(options, OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}