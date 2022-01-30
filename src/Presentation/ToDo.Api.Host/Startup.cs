using System;
using System.Net.Http;
using System.Security.Claims;
using Autofac;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Logging;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.Core;
using EventFlow.EventStores.EventStore;
using EventFlow.Extensions;
using EventFlow.Logs;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SN.Api.Server;
using SN.Api.Server.Common;
using SN.Api.Server.Swagger;
using SN.Api.Server.Swagger.Extensions;
using ToDo.Api.Contract.ToDo;
using ToDo.Api.Host.Auth;
using ToDo.Api.Host.Doc;
using ToDo.Api.Host.Jobs;
using ToDo.Core.Module;
using ToDo.ReadStore.EF.Module;

namespace ToDo.Api.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IApiServiceProvider>(_ =>
                    {
                        return new DefaultApiServiceProvider(new[]
                        {
                            typeof(IToDo)
                        });
                    })
                    .AddMvcCore()
                    .AddApiServices();
            
            ConfigureAuthentication(services);
            
            services.AddScoped<IToDo, Services.ToDo>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ToDo.Api.Host", Version = "v1"});
                c.IncludeXmlComments(typeof(IToDo).Assembly.DocumentationFilePath());
                c.DocumentFilter<ApiServiceDocumentFilter>();
                c.DocumentFilter<EventsDocumentFilter>();
            });
            services.AddHostedService<DBMigratorService>();
            services.AddHostedService<ModelPopulatorService>();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        _configuration.GetSection("Auth:OIDC").Bind(options);

                        options.TokenValidationParameters.ValidateAudience = false;
                        
                        options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;
                        options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                    });
            services.Configure<MvcOptions>(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            var eventFlowOptions = EventFlowOptions.New.UseAutofacContainerBuilder(containerBuilder);

            eventFlowOptions.Configure(configuration =>
            {
                configuration.PopulateReadModelEventPageSize = 1000;
            });
            eventFlowOptions.AddAspNetCore(options =>
            {
                options.UseDefaults();
            });
            eventFlowOptions.AddUserNameMetadata(ClaimTypes.NameIdentifier);
            eventFlowOptions.RegisterServices(registration =>
            {
                registration.Register<ILog, AspNetCoreLoggerLog>();
            });

            var esConfig = _configuration.GetSection("EventStore:EventStoreDb");
            var esUri = esConfig.GetValue<Uri>("ConnectionString");
            var esUriBuilder = new UriBuilder(esUri);
            var esHttpMessageHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = delegate { return true; }
            };
            var esUserCredentials = new UserCredentials(esUriBuilder.UserName, esUriBuilder.Password);
            var esSettings = ConnectionSettings.Create()
                                               .KeepRetrying()
                                               .KeepReconnecting()
                                               .UseConsoleLogger()
                                               .SetDefaultUserCredentials(esUserCredentials)
                                               .DisableServerCertificateValidation()
                                               .UseCustomHttpMessageHandler(esHttpMessageHandler);

            var esClusterSettings = ClusterSettings.Create()
                                                   .DiscoverClusterViaDns()
                                                   .KeepDiscovering()
                                                   .SetMaxDiscoverAttempts(500)
                                                   .SetClusterDns(esUri.Host);
            
            var connection = EventStoreConnection.Create(esSettings, esClusterSettings);

            using (var bridge = AsyncHelper.Wait)
            {
                bridge.Run(connection.ConnectAsync());
            }

            eventFlowOptions.RegisterServices(f => f.Register(r => connection, Lifetime.Singleton));
            eventFlowOptions.UseEventStore<EventStoreEventPersistence>();
            
            eventFlowOptions.RegisterModule<ToDoDomainModule>();
            eventFlowOptions.RegisterModule<ToDoStoreModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResolver resolver)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo.Api.Host v1"));
            }

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}