using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;
using Microsoft.AspNetCore.Http;

namespace ToDo.Api.Auth
{
    internal static class UserNameMetadataProviderExtensions
    {
        public static IEventFlowOptions AddUserNameMetadata(this IEventFlowOptions options, string userNameClaimType)
        {
            options.AddMetadataProvider<UserNameMetadataProvider>();
            options.RegisterServices(services =>
            {
                services.Register(_ => new UserNameMetadataProviderOptions
                {
                    UserNameClaimType = userNameClaimType
                });
                services.Register<IHttpContextAccessor, HttpContextAccessor>(Lifetime.Singleton, true);
            });
            
            return options;
        }
    }
}