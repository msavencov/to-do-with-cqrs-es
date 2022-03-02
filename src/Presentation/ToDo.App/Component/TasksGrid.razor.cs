using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDo.Api.Client.ToDoApi.Client;
using ToDo.Api.Client.ToDoApi.Services;
using ToDo.Api.Contract.List;
using ToDo.Api.Contract.List.Models;
using ToDo.Api.Contract.Tasks.Models;
using ToDo.Api.Contract.Tasks.Operations;

namespace ToDo.App.Component
{
    public partial class TasksGrid
    {
        [Inject] private IToDoApi Api { get; set; }
        
        [Parameter] public ToDoList List { get; set; }
        [Parameter] public ToDoItemCollection Tasks { get; set; }

        private async Task TaskItemDone(ToDoItem item)
        {
            await Api.CompleteTaskAsync(new CompleteTask
            {
                TaskId = item.Id,
            });
        }

        private async Task TaskItemDelete(ToDoItem item)
        {
            await Api.DeleteTaskAsync(new DeleteTask
            {
                TaskId = item.Id
            });
        }
    }
}