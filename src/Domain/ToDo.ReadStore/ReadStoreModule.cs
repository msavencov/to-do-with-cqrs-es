using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;

namespace ToDo.ReadStore
{
    public class ReadStoreModule : IModule
    {
        public void Register(IEventFlowOptions eventFlowOptions)
        {
            eventFlowOptions.AddDefaults(typeof(ReadStoreModule).Assembly);
        }
    }
}