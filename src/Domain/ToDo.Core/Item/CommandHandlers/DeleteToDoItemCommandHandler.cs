using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using ToDo.Core.Item.Commands;

namespace ToDo.Core.Item.CommandHandlers
{
    public class DeleteToDoItemCommandHandler : CommandHandler<ToDoItem, ToDoItemId, DeleteToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, DeleteToDoItem command, CancellationToken cancellationToken)
        {
            aggregate.Delete();
            
            return Task.CompletedTask;
        }
    }
}