using EventFlow;
using EventFlow.Configuration;

namespace ToDo.Infrastructure
{
    public class EventPublisherModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            //options.PublishToRabbitMq();
        }
    }
}