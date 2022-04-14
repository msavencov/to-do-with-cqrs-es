using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace ToDo.Core.List.Commands
{
    public class ChangeListNameHandler : CommandHandler<ToDoList, ToDoListId, ChangeListName>
    {
        public override Task ExecuteAsync(ToDoList aggregate, ChangeListName command, CancellationToken cancellationToken)
        {
            aggregate.ChangeName(command.Name);

            return Task.CompletedTask;
        }
    }
}