using System;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ToDo.Api.Client.Auth;
using ToDo.Api.Client.Configuration;
using ToDo.Api.Contract.Lists;
using ToDo.Api.Contract.Tasks;

namespace ToDo.Api.Client
{
    public static class ClientModule
    {
        private static TOptions GetRequiredOptions<TOptions>(this IServiceProvider provider) where TOptions : class
        {
            return provider.GetRequiredService<IOptions<TOptions>>().Value;
        }
        
        public static IServiceCollection AddGrpcClient(this IServiceCollection services, Action<ToDoApiOptions> configureOptions)
        {
            services.Configure(configureOptions);
            
            services.AddGrpcClient<TasksService.TasksServiceClient>((provider, options) =>
                    {
                        options.Address = provider.GetRequiredOptions<ToDoApiOptions>().BaseAddress;
                    })
                    .AddInterceptor<AuthInterceptor>(InterceptorScope.Client);
            
            services.AddGrpcClient<ListService.ListServiceClient>((provider, options) =>
                    {
                        options.Address = provider.GetRequiredOptions<ToDoApiOptions>().BaseAddress;
                    })
                    .AddInterceptor<AuthInterceptor>(InterceptorScope.Client);
            
            services.AddScoped<AuthInterceptor>();
            
            return services;
        }
    }
}