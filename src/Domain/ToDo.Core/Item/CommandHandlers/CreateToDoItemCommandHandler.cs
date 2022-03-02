using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using ToDo.Core.Item.Commands;

namespace ToDo.Core.Item.CommandHandlers
{
    public class CreateToDoItemCommandHandler : CommandHandler<ToDoItem, ToDoItemId, CreateToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, CreateToDoItem command, CancellationToken cancellationToken)
        {
            aggregate.Create(command.ListId, command.Task);
            
            return Task.CompletedTask;
        }
    }
}