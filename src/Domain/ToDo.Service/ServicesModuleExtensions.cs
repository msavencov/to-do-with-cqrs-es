using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Service
{
    public static class ServicesModuleExtensions
    {
        public static IServiceCollection AddToDoApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServicesModuleExtensions).Assembly);
            
            return services;
        }
    }
}