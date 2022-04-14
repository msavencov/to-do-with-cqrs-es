using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace ToDo.Core.Item.Commands
{
    public class CompleteToDoItemHandler : CommandHandler<ToDoItem, ToDoItemId, CompleteToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, CompleteToDoItem command, CancellationToken ct)
        {
            aggregate.Complete();
            
            return Task.CompletedTask;
        }
    }
}