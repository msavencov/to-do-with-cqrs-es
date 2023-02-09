using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using ToDo.Api.Contract.Tasks;

namespace ToDo.App.Component.TaskComponent
{
    public partial class TasksGrid
    {
        [Inject] public DialogService DialogService { get; private set; }

        [Parameter] public string ListId { get; set; }
        
        private ApiCallWrapper<TasksService.TasksServiceClient> TaskApiWrapper { get; set; }
        private FindRequest.Types.Response Tasks;
        private RadzenDataGrid<TaskItem> Grid;
        
        protected override async Task OnParametersSetAsync()
        {
            if (Grid is {})
            {
                Grid.Reset();
                await Grid.Reload();
            }
        }

        private async Task TaskItemDone(TaskItem item)
        {
            await TaskApiWrapper.Execute(async t =>
            {
                await t.CompleteAsync(new CompleteRequest
                {
                    Id = item.Id,
                });
            });
            await Grid.Reload();
        }

        private async Task TaskItemDelete(TaskItem item)
        {
            await TaskApiWrapper.Execute(async t =>
            {
                await t.DeleteAsync(new DeleteRequest
                {
                    Id = item.Id,
                });
            });
            await Grid.Reload();
        }

        private async Task AddTaskClick(MouseEventArgs arg)
        {
            var options = new DialogOptions
            {
                ShowClose = false
            };
            var callback = EventCallback.Factory.Create<string>(this, OnTaskCreated);
            var parameters = new AddTaskDialog.DialogParams().WithListId(ListId).WithOnTaskCreated(callback);

            await DialogService.OpenAsync<AddTaskDialog>("Add New Task", parameters, options);
        }

        private async Task OnTaskCreated(string taskId)
        {
            await Grid.Reload();
        }

        private async Task OnLoadData(LoadDataArgs arg)
        {
            Tasks = null;

            if (ListId is { Length: > 0 })
            {
                Tasks = await TaskApiWrapper.Execute(async t =>
                {
                    var request = new FindRequest
                    {
                        ListId = ListId
                    };
                    return await t.FindAsync(request);
                });
            }
        }
    }
}