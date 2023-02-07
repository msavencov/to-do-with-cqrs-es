using EventFlow;
using EventFlow.Configuration;
using EventFlow.EntityFramework.Extensions;

namespace ToDo.ReadStore.EF.SQLServer.Module
{
    public class SQLServerStoreModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            options.AddDbContextProvider<ToDoContext, SQLServerContextProvider>();
        }
    }
}