using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;

namespace ToDo.Core.Module
{
    public class ToDoDomainModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            options.AddDefaults(typeof(ToDoDomainModule).Assembly);
        }
    }
}