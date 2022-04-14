using EventFlow;
using EventFlow.Configuration;

namespace ToDo.ReadStore.Abstractions
{
    public class EventPublisherModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            //options.PublishToRabbitMq();
        }
    }
}