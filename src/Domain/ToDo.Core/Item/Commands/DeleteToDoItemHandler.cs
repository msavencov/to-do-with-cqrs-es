using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace ToDo.Core.Item.Commands
{
    public class DeleteToDoItemHandler : CommandHandler<ToDoItem, ToDoItemId, DeleteToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, DeleteToDoItem command, CancellationToken cancellationToken)
        {
            aggregate.Delete();
            
            return Task.CompletedTask;
        }
    }
}