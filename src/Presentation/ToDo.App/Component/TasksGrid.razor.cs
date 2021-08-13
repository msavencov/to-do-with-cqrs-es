using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDo.Api.Contract.ToDo;
using ToDo.Api.Contract.ToDo.Models;

namespace ToDo.App.Component
{
    public partial class TasksGrid
    {
        [Inject] private IToDo Api { get; set; }
        
        [Parameter] public ToDoList List { get; set; }
        [Parameter] public ToDoItemCollection Tasks { get; set; }

        private async Task TaskItemDone(ToDoItem item)
        {
            await Api.TaskDone(item.Id);
        }

        private async Task TaskItemDelete(ToDoItem item)
        {
            await Api.DeleteTask(item.Id);
        }
    }
}