using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;

namespace ToDo.Service.Module
{
    public class ToDoServiceModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            options.AddDefaults(typeof(ToDoServiceModule).Assembly);
        }
    }
}