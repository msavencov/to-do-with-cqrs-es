using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using ToDo.Core.List.Commands;

namespace ToDo.Core.List
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