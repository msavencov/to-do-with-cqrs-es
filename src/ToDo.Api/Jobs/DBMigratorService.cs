using System.Threading;
using System.Threading.Tasks;
using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ToDo.ReadStore.EF;

namespace ToDo.Api.Jobs
{
    internal class DBMigratorService : IHostedService
    {
        private readonly IDbContextProvider<ToDoContext> _contextProvider;
        
        public DBMigratorService(IDbContextProvider<ToDoContext> contextProvider)
        {
            _contextProvider = contextProvider;
        }
        
        public async Task StartAsync(CancellationToken ct)
        {
            await using var context = _contextProvider.CreateContext();
            
            if (context.Database.IsInMemory())
            {
                return;
            }
            
            await context.Database.EnsureCreatedAsync(ct);
            await context.Database.MigrateAsync(ct);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}