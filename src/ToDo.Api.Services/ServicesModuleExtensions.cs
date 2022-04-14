using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Service
{
    public static class ServicesModuleExtensions
    {
        public static IServiceCollection AddToDoApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}