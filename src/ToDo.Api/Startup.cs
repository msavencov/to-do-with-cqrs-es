using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Logging;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.EventStores.EventStore.Extensions;
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
using ToDo.Api.Auth;
using ToDo.Api.Contract;
using ToDo.Api.Doc;
using ToDo.Api.Jobs;
using ToDo.Api.Services;
using ToDo.Api.Services.List;
using ToDo.Api.Services.Tasks;
using ToDo.Api.Utils;
using ToDo.Api.Validation;
using ToDo.Core.Module;
using ToDo.ReadStore.EF.InMemory;
using ToDo.ReadStore.EF.Module;
using ToDo.ReadStore.EF.PostgreSQL.Module;
using ToDo.ReadStore.EF.SQLServer.Module;

namespace ToDo.Api
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            
            ConfigureAuthentication(services);
            ConfigureSwagger(services);
            ConfigureGrpc(services);
            services.AddToDoApplication();
            
            services.AddHostedService<DBMigratorService>();
            services.AddHostedService<ModelPopulatorService>();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            var eventFlowOptions = EventFlowOptions.New.UseAutofacContainerBuilder(containerBuilder);

            eventFlowOptions.RegisterServices(registration =>
            {
                registration.Decorate<ICommandBus>((c, inner) => new StatsCommandBus(inner));
            });
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
            var esConnectionString = esConfig.GetValue<string>("ConnectionString");

            if (esConnectionString.StartsWith("ConnectTo=", StringComparison.OrdinalIgnoreCase))
            {
                var esSettings = ConnectionString.GetConnectionSettings(esConnectionString);
                var esConnectionBuilder = new DbConnectionStringBuilder(false)
                {
                    ConnectionString = esConnectionString,
                };
                
                if (esConnectionBuilder.TryGetValue("ConnectTo", out var esUriString) == false)
                {
                    throw new ArgumentException("The ConnectionString doesn't contain the 'ConnectTo' argument.");
                }
                var esUri = new Uri((string)esUriString);

                eventFlowOptions.UseEventStoreEventStore(esUri, esSettings);
            }

            if (esConnectionString.StartsWith("discover", StringComparison.OrdinalIgnoreCase))
            {
                var esUri = new Uri(esConnectionString); 
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
                
                eventFlowOptions.UseEventStoreEventStore(esUri, esSettings);
            }

            eventFlowOptions.RegisterModule<ToDoDomainModule>();
            eventFlowOptions.RegisterModule<ToDoStoreModule>();

            var readStoreSettings = _configuration.GetSection("ToDoContext");
            var readStoreProvider = readStoreSettings.GetValue<string>("UseProvider");

            switch (readStoreProvider)
            {
                case "PostgreSQL":
                    eventFlowOptions.RegisterModule<PostgreSQLStoreModule>();
                    break;
                case "SQLServer":
                    eventFlowOptions.RegisterModule<SQLServerStoreModule>();
                    break;
                default:
                    eventFlowOptions.RegisterModule<InMemoryStoreModule>();
                    break;
            }
            
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
                    });
            services.Configure<MvcOptions>(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        private void ConfigureGrpc(IServiceCollection services)
        {
            services.AddGrpc();
        }
        
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ToDo.Api.Host", Version = "v1"});
                c.CustomSchemaIds(type => type.FullName);
                c.DocumentFilter<EventsDocumentFilter>();

                c.IncludeXmlComments(typeof(ContractModule).Assembly.DocumentationFilePath());
                c.IncludeXmlComments(typeof(Startup).Assembly.DocumentationFilePath());
                
                var authority = _configuration.GetValue<string>("Auth:OIDC:Authority");
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = new Uri($"{authority}/.well-known/openid-configuration"),
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" },
                                { "email", "email" },
                                { "roles", "roles" },
                                { "profile", "profile" },
                            }
                        }
                    }
                };
                c.AddSecurityDefinition("oidc", securityScheme);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResolver resolver, IEventStoreConnection esConnection)
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
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                endpoints.MapGrpcService<ListService>().RequireAuthorization();
                endpoints.MapGrpcService<TasksService>().RequireAuthorization();
            });
        }
    }
}