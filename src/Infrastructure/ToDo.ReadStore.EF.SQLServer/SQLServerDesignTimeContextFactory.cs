using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ToDo.ReadStore.EF
{
    // dotnet ef migrations add "name" --project Infrastructure\ToDo.ReadStore.EF\ToDo.ReadStore.EF.csproj --startup-project Presentation\ToDo.Api.Host\ToDo.Api.Host.csproj
    // dotnet ef migrations remove --project Infrastructure\ToDo.ReadStore.EF\ToDo.ReadStore.EF.csproj --startup-project Presentation\ToDo.Api.Host\ToDo.Api.Host.csproj
    public sealed class SQLServerDesignTimeContextFactory : IDesignTimeDbContextFactory<ToDoContext>
    {
        public ToDoContext CreateDbContext(string[] args)
        {
            for (var index = 0; index < args.Length; index++)
            {
                Console.WriteLine($"ARG[{index}]: {args[index]}");
            }

            var cb = new DbContextOptionsBuilder<ToDoContext>();
            
            cb.UseSqlServer("Server=localhost;Database=ToDo;Integrated Security=True");
            
            return new ToDoContext(cb.Options);
        }
    }
}