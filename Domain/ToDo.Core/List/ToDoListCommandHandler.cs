using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using ToDo.Core.List.Commands;

namespace ToDo.Core.List
{
    public class ToDoListCommandHandler : CommandHandler<ToDoList, ToDoListId, CreateToDoList>
    {
        public override Task ExecuteAsync(ToDoList aggregate, CreateToDoList command, CancellationToken cancellationToken)
        {
            aggregate.Create(command.ListName);
            
            return Task.CompletedTask;
        }
    }
}