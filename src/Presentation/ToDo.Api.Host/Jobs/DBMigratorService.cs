using System.Threading;
using System.Threading.Tasks;
using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ToDo.ReadStore.EF;

namespace ToDo.Api.Host.Jobs
{
    public class DBMigratorService : IHostedService
    {
        private readonly IDbContextProvider<ToDoContext> _contextProvider;
        
        public DBMigratorService(IDbContextProvider<ToDoContext> contextProvider)
        {
            _contextProvider = contextProvider;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _contextProvider.CreateContext().Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}