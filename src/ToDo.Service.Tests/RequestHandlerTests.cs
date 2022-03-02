using System.Linq;
using MediatR;
using ToDo.Api.Contract;
using ToDo.Service.Tests.Extensions;
using Xunit;

namespace ToDo.Service.Tests
{
    public class RequestHandlerTests
    {
        [Fact]
        public void AllRequestsShouldHaveExactlyOneHandler()
        {
            var requests = from t in typeof(ServiceModule).Assembly.GetTypes()
                           where t.IsAbstract == false
                           from i in t.GetInterfaces()
                           where i.IsGenericType
                           where i.GetGenericTypeDefinition() == typeof(IRequest<>)
                           select new
                           {
                               Request = t,
                               Response = i.GenericTypeArguments.Single()
                           };
            var implementations = typeof(ServicesModule).Assembly.GetTypes();
                                  
            foreach (var request in requests)
            {
                var service = typeof(IRequestHandler<,>).MakeGenericType(request.Request, request.Response);
                var handlers = implementations.Where(t => t.GetInterfaces().Any(i => i == service)).ToArray();
            
                Assert.True(handlers is {Length: 1}, $"The '{request.Request.Name}' must have exactly one handler defined. Implement interface: {service.ShortName()}");
            }
        }
    }
}