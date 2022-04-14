using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.ReadStores;
using Microsoft.Extensions.Hosting;
using ToDo.ReadStore.Items;
using ToDo.ReadStore.Lists;

namespace ToDo.Api.Host.Jobs
{
    internal class ModelPopulatorService : IHostedService
    {
        private readonly IReadModelPopulator _modelPopulator;

        public ModelPopulatorService(IReadModelPopulator modelPopulator)
        {
            _modelPopulator = modelPopulator;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();

            Console.WriteLine($"Updating model {typeof(ToDoListReadModel)}");
            await _modelPopulator.PopulateAsync<ToDoListReadModel>(cancellationToken);
            Console.WriteLine($"Updated model {typeof(ToDoListReadModel)} in {sw.Elapsed}");
            
            sw.Restart();
            
            Console.WriteLine($"Updating model {typeof(ToDoItemReadModel)}");
            await _modelPopulator.PopulateAsync<ToDoItemReadModel>(cancellationToken);
            Console.WriteLine($"Updated model {typeof(ToDoItemReadModel)} in {sw.Elapsed}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}