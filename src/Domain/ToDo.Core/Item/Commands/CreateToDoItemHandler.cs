using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace ToDo.Core.Item.Commands
{
    public class CreateToDoItemHandler : CommandHandler<ToDoItem, ToDoItemId, CreateToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, CreateToDoItem command, CancellationToken cancellationToken)
        {
            aggregate.Create(command.ListId, command.Task);
            
            return Task.CompletedTask;
        }
    }
}