using Microsoft.EntityFrameworkCore;
using ToDo.ReadStore.ToDo;

namespace ToDo.ReadStore.EF
{
    // dotnet ef migrations add "name" --project Infrastructure\ToDo.ReadStore.EF\ToDo.ReadStore.EF.csproj --startup-project Presentation\ToDo.Api.Host\ToDo.Api.Host.csproj
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