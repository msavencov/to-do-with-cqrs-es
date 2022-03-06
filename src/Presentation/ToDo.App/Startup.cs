using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using ToDo.Api.Client;
using ToDo.Api.Client.Infrastructure;
using ToDo.App.Auth;

namespace ToDo.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();
            
            
            services.AddToDoApiClient(options =>
            {
                options.BaseAddress = new Uri("http://localhost:14900");
            });
            services.AddScoped<TokenProvider>();
            services.AddScoped<IAccessTokenAccessor, AccessTokenAccessor>();
            
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            ConfigureAuthenticationServices(services);
        }

        private void ConfigureAuthenticationServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                    {
                        var oidc = _configuration.GetSection("Auth:OIDC");
                        var users = _configuration.GetSection("Auth:Users").AsEnumerable().Select(t => t.Value).ToArray();
                    
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;

                        options.Authority = oidc["Authority"];
                        options.ClientId = oidc["ClientId"];
                        options.ClientSecret = oidc["ClientSecret"];

                        options.ResponseType = "code";
                    
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("roles");
                        options.Scope.Add("email");
                        options.Scope.Add("offline_access");

                        options.SaveTokens = true;
                        options.GetClaimsFromUserInfoEndpoint = true;
                    
                        options.Events ??= new OpenIdConnectEvents();
                        options.Events.OnTokenValidated = context =>
                        {
                            if (context.Principal is { })
                            {
                                var nameIdentifier = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                                var name = nameIdentifier?.Replace(@"starnet\", "", StringComparison.OrdinalIgnoreCase);
                            
                                var claims = new List<Claim>
                                {
                                    new(ClaimTypes.Role, "user")
                                };
                                if (users.Contains(name))
                                {
                                    claims.Add(new (ClaimTypes.Role, "admin"));
                                }
                            
                                context.Principal.AddIdentity(new ClaimsIdentity(claims));
                            }
                        
                            return Task.CompletedTask;
                        };
                    });
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var forwardedHeadersOptions = new ForwardedHeadersOptions
            {
                ForwardLimit = null,
                ForwardedHeaders = ForwardedHeaders.All
            };
            forwardedHeadersOptions.AllowedHosts.Clear();
            forwardedHeadersOptions.KnownNetworks.Clear();
            forwardedHeadersOptions.KnownProxies.Clear();
            
            app.UseForwardedHeaders(forwardedHeadersOptions);
            
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}