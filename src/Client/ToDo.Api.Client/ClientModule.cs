using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ToDo.Api.Client.Configuration;
using ToDo.Api.Client.Infrastructure;
using ToDo.Api.Client.ToDoApi;
using ToDo.Api.Client.ToDoApi.Extensions;

namespace ToDo.Api.Client
{
    public static class ClientModule
    {
        public static IServiceCollection AddToDoApiClient(this IServiceCollection services, Action<ToDoApiOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddToDoApi(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });
            services.AddHttpClient<IToDoApiRequestHandler, ToDoApiRequestHandler>()
                    .ConfigureHttpClient((provider, client) =>
                    {
                        client.BaseAddress = provider.GetRequiredService<IOptions<ToDoApiOptions>>().Value.BaseAddress;
                    })
                    .ConfigurePrimaryHttpMessageHandler(provider =>
                    {
                        return new HttpClientHandler
                        {
                            ClientCertificateOptions = ClientCertificateOption.Manual,
                            ServerCertificateCustomValidationCallback = delegate { return true; }
                        };
                    });
            
            return services;
        }
    }
}