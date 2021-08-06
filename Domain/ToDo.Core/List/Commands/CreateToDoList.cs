using EventFlow.Commands;

namespace ToDo.Core.List.Commands
{
    public class CreateToDoList : Command<ToDoList, ToDoListId>
    {
        public string ListName { get; }

        public CreateToDoList(string listName) : base(ToDoListId.New)
        {
            ListName = listName;
        }
    }
}