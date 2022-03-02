using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using ToDo.Core.Item.Commands;

namespace ToDo.Core.Item.CommandHandlers
{
    public class RenameToDoItemCommandHandler : CommandHandler<ToDoItem, ToDoItemId, RenameToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, RenameToDoItem command, CancellationToken cancellationToken)
        {
            aggregate.Rename(command.Name);
            
            return Task.CompletedTask;
        }
    }
}