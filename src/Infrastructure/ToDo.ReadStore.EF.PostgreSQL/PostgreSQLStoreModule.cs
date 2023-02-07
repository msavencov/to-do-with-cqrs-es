using EventFlow;
using EventFlow.Configuration;
using EventFlow.EntityFramework.Extensions;

namespace ToDo.ReadStore.EF.PostgreSQL.Module
{
    public class PostgreSQLStoreModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            options.AddDbContextProvider<ToDoContext, PostgreSQLContextProvider>();
        }
    }
}