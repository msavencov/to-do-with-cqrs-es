using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace ToDo.Core.List.Commands
{
    public class CreateToDoListHandler : CommandHandler<ToDoList, ToDoListId, CreateToDoList>
    {
        public override Task ExecuteAsync(ToDoList aggregate, CreateToDoList command, CancellationToken cancellationToken)
        {
            aggregate.Create(command.ListName);
            
            return Task.CompletedTask;
        }
    }
}