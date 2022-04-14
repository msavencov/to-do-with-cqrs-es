using Microsoft.EntityFrameworkCore;
using ToDo.ReadStore.Items;
using ToDo.ReadStore.Lists;

namespace ToDo.ReadStore.EF
{
    // dotnet ef migrations add "name" --project Infrastructure\ToDo.ReadStore.EF\ToDo.ReadStore.EF.csproj --startup-project Presentation\ToDo.Api.Host\ToDo.Api.Host.csproj
    // dotnet ef migrations remove --project Infrastructure\ToDo.ReadStore.EF\ToDo.ReadStore.EF.csproj --startup-project Presentation\ToDo.Api.Host\ToDo.Api.Host.csproj
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ToDoListReadModel>().HasKey(t => t.Id);
            builder.Entity<ToDoItemReadModel>().HasKey(t => t.Id);
        }
    }
}