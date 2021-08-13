using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using ToDo.Core.Item.Commands;

namespace ToDo.Core.Item.CommandHandlers
{
    public class CompleteToDoItemCommandHandler : CommandHandler<ToDoItem, ToDoItemId, CompleteToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, CompleteToDoItem command, CancellationToken ct)
        {
            aggregate.Complete();
            
            return Task.CompletedTask;
        }
    }
}