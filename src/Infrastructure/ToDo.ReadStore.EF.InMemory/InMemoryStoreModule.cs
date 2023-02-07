using EventFlow;
using EventFlow.Configuration;
using EventFlow.EntityFramework.Extensions;

namespace ToDo.ReadStore.EF.InMemory;

public class InMemoryStoreModule : IModule
{
    public void Register(IEventFlowOptions eventFlowOptions)
    {
        eventFlowOptions.AddDbContextProvider<ToDoContext, InMemoryContextProvider>();
    }
}