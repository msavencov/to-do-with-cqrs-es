using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Api.Services
{
    public static class ServicesModuleExtensions
    {
        public static IServiceCollection AddToDoApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}