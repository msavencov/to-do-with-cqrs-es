using System;
using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ToDo.ReadStore.EF.InMemory
{
    internal class InMemoryContextProvider : IDbContextProvider<ToDoContext>
    {
        private readonly DbContextOptionsBuilder<ToDoContext> _optionsBuilder;

        public InMemoryContextProvider()
        {
            var assemblyName = typeof(ToDoContext).Assembly.GetName().Name;

            _optionsBuilder = new DbContextOptionsBuilder<ToDoContext>();
            _optionsBuilder.UseInMemoryDatabase(assemblyName);
    }
        
        public ToDoContext CreateContext()
        {
            return new ToDoContext(_optionsBuilder.Options);
        }
    }

}