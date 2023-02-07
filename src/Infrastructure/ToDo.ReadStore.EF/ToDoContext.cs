using Microsoft.EntityFrameworkCore;
using ToDo.ReadStore.Items;
using ToDo.ReadStore.Lists;

namespace ToDo.ReadStore.EF
{
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