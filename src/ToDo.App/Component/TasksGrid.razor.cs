using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDo.Api.Contract.Lists;
using ToDo.Api.Contract.Tasks;

namespace ToDo.App.Component
{
    public partial class TasksGrid
    {
        [Inject] private TasksService.TasksServiceClient Api { get; set; }
        
        [Parameter] public ListItem List { get; set; }
        [Parameter] public Api.Contract.Tasks.FindRequest.Types.Response Tasks { get; set; }

        private async Task TaskItemDone(TaskItem item)
        {
            await Api.CompleteAsync(new CompleteRequest
            {
                Id = item.Id,
            });
        }

        private async Task TaskItemDelete(TaskItem item)
        {
            await Api.DeleteAsync(new DeleteRequest
            {
                Id = item.Id,
            });
        }
    }
}