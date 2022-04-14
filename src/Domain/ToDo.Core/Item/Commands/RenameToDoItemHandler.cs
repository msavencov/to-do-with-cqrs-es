using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace ToDo.Core.Item.Commands
{
    public class RenameToDoItemHandler : CommandHandler<ToDoItem, ToDoItemId, RenameToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, RenameToDoItem command, CancellationToken cancellationToken)
        {
            aggregate.Rename(command.Name);
            
            return Task.CompletedTask;
        }
    }
}