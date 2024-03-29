﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToDo.App.Pages.Account
{
    public class Login : PageModel
    {
        public IActionResult OnGet(string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.Content("~/");
            }
            
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                Response.Redirect(redirectUri ?? "/");
            }

            var challengeOptions = new AuthenticationProperties {RedirectUri = redirectUri};
            var authScheme = OpenIdConnectDefaults.AuthenticationScheme;
            
            return Challenge(challengeOptions, authScheme);
        }
    }
}