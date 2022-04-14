using EventFlow;
using EventFlow.Configuration;
using EventFlow.EntityFramework;
using EventFlow.EntityFramework.Extensions;
using EventFlow.Extensions;
using EventFlow.ReadStores;
using ToDo.ReadStore.Abstractions.ReadStores;
using ToDo.ReadStore.Items;
using ToDo.ReadStore.Lists;

namespace ToDo.ReadStore.EF.Module
{
    public class ToDoStoreModule : IModule
    {
        public void Register(IEventFlowOptions options)
        {
            options.ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                   .AddDefaults(typeof(ToDoStoreModule).Assembly)
                   .AddDbContextProvider<ToDoContext, ToDoContextProvider>()
                   .UseEntityFrameworkReadModel<ToDoListReadModel, ToDoContext, ToDoListReadModelLocator>()
                   .UseEntityFrameworkReadModel<ToDoItemReadModel, ToDoContext>()
                   .UseEntityFrameworkSnapshotStore<ToDoContext>()
                   .RegisterServices(t =>
                   {
                       t.Register<IReadModelLocator, ToDoListReadModelLocator>();
                       t.Register<IReadModelPopulator, ReadModelPopulator>();
                       t.Register<ISearchableReadModelStore<ToDoListReadModel>, EfSearchableReadStore<ToDoListReadModel, ToDoContext>>();
                       t.Register<ISearchableReadModelStore<ToDoItemReadModel>, EfSearchableReadStore<ToDoItemReadModel, ToDoContext>>();
                   });
        }
    }
}