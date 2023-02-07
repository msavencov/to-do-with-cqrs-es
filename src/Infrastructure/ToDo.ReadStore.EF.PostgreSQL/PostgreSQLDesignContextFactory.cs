using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ToDo.ReadStore.EF.PostgreSQL
{
    public class PostgreSQLDesignContextFactory : IDesignTimeDbContextFactory<ToDoContext>
    {
        public ToDoContext CreateDbContext(string[] args)
        {
            for (var index = 0; index < args.Length; index++)
            {
                Console.WriteLine($"ARG[{index}]: {args[index]}");
            }

            var connectionString = Environment.GetEnvironmentVariable("ToDoDesignTimeContext:ConnectionString");
            if (connectionString is null)
            {
                throw new ArgumentException("The 'ToDoDesignTimeContext:ConnectionString' environment variable is not set.");
            }
            var contextBuilder = new DbContextOptionsBuilder<ToDoContext>();
            
            contextBuilder.UseNpgsql(connectionString);
            
            return new ToDoContext(contextBuilder.Options);
        }
    }
}