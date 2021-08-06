using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ToDo.ReadStore.EF
{
    public class ToDoDesignTimeContextFactory : IDesignTimeDbContextFactory<ToDoContext>
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