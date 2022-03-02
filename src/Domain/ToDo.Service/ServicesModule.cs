using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;

namespace ToDo.Service
{
    public class ServicesModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            options.AddDefaults(typeof(ServicesModule).Assembly);
        }
    }
}