using EventFlow.Commands;

namespace ToDo.Core.List.Commands
{
    public class ChangeListName : Command<ToDoList, ToDoListId>
    {
        public string Name { get; }

        public ChangeListName(string id, string name) : base(ToDoListId.With(id))
        {
            Name = name;
        }
    }
}