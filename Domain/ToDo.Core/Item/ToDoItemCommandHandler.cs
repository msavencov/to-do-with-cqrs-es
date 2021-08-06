using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using ToDo.Core.Item.Commands;

namespace ToDo.Core.Item
{
    public class ToDoItemCommandHandler : CommandHandler<ToDoItem, ToDoItemId, CreateToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, CreateToDoItem command, CancellationToken cancellationToken)
        {
            aggregate.Create(command.ListId, command.Task);
            
            return Task.CompletedTask;
        }
    }
    
    public class CompleteToDoItemCommandHandler : CommandHandler<ToDoItem, ToDoItemId, CompleteToDoItem>
    {
        public override Task ExecuteAsync(ToDoItem aggregate, CompleteToDoItem command, CancellationToken ct)
        {
            aggregate.Complete();
            
            return Task.CompletedTask;
        }
    }
}