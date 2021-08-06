using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.EventStores;
using EventFlow.EventStores.Files;
using EventFlow.Extensions;
using EventFlow.Queries;
using EventFlow.ReadStores;
using ToDo.Core;
using ToDo.Core.List.Commands;
using c = System.Console;

namespace ToDo.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ContainerBuilder();
            services.RegisterType<ToDoReadModelLocator>();
            
            var options = EventFlowOptions.New;
            var domainAssembly = typeof(ToDoReadModel).Assembly;

            options.UseAutofacContainerBuilder(services);
            options.UseFilesEventStore(FilesEventStoreConfiguration.Create("./event-store.db"));
            options.AddDefaults(domainAssembly);
            options.UseInMemoryReadStoreFor<ToDoReadModel, ToDoReadModelLocator>();
            
            using (var resolver = options.CreateResolver())
            {
                var cb = resolver.Resolve<ICommandBus>();
                var qp = resolver.Resolve<IQueryProcessor>();
                
                while (true)
                {
                    var cmd = Prompt("Enter one of commands: 'list', 'list-create', 'list', 'q': ");

                    switch (cmd)
                    {
                        case "list":
                            var id = Prompt("Enter list id: ");
                            var list = await qp.ProcessAsync(new ReadModelByIdQuery<ToDoReadModel>(id), default);
                            
                            break;
                        case "list-create":
                            var listName = Prompt("Enter list name: ");
                            var createToDoList = new CreateToDoList(listName);
                            var result = await cb.PublishAsync(createToDoList, CancellationToken.None);
                            
                            if (result.IsSuccess)
                            {
                                c.WriteLine($"Created list: {listName} with id: {createToDoList.AggregateId}");
                            }
                            
                            break;
                    }
                }
            }
        }

        private static string Prompt(string message)
        {
            c.Write(message);
            return c.ReadLine();
        }
    }
}