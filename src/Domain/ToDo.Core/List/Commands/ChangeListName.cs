using EventFlow.Commands;

namespace ToDo.Core.List.Commands
{
    public class ChangeListName : Command<ToDoList, ToDoListId>
    {
        public string Name { get; }

        public ChangeListName(ToDoListId id, string name) : base(id)
        {
            Name = name;
        }
    }
}