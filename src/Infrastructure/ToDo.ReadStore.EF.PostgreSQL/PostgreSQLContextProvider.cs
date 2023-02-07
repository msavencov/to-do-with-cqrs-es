using System;
using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ToDo.ReadStore.EF.PostgreSQL
{
    public class PostgreSQLContextProvider : IDbContextProvider<ToDoContext>
    {
        private readonly DbContextOptionsBuilder<ToDoContext> _optionsBuilder;

        public PostgreSQLContextProvider(IConfiguration configuration)
        {
            var config = configuration.GetSection("ToDoContext:PostgreSQL").Get<ToDoContextOptions>();
            var assemblyName = typeof(ToDoContext).Assembly.GetName().Name;
            
            _optionsBuilder = new DbContextOptionsBuilder<ToDoContext>();
            _optionsBuilder.UseNpgsql(config.ConnectionString, builder =>
            {
                builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(1), default);
                builder.MigrationsAssembly(assemblyName);
                builder.MigrationsHistoryTable($"__Migrations.{assemblyName}");
            });
        }
        
        public ToDoContext CreateContext()
        {
            return new ToDoContext(_optionsBuilder.Options);
        }

        public class ToDoContextOptions
        {
            public string ConnectionString { get; set; }
        }
    }

}