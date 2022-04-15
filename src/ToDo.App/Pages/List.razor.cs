using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDo.Api.Contract.Lists;
using ToDo.Api.Contract.Tasks;
using ToDo.App.Shared;
using AddRequest = ToDo.Api.Contract.Lists.AddRequest;
using FindRequest = ToDo.Api.Contract.Tasks.FindRequest;
using GetTaskRequest = ToDo.Api.Contract.Tasks.GetRequest;
using GetListRequest = ToDo.Api.Contract.Lists.GetRequest;
using RenameRequest = ToDo.Api.Contract.Lists.RenameRequest;

namespace ToDo.App.Pages
{
    public partial class List
    {
        [Inject] public ListService.ListServiceClient ListApi { get; private set; }
        [Inject] public TasksService.TasksServiceClient TaskApi { get; private set; }
        [Inject] public NavigationManager NavigationManager { get; private set; }
        
        [CascadingParameter] public Error Error { get; set; }
        
        private Api.Contract.Lists.FindRequest.Types.Response Lists { get; set; }
        private Api.Contract.Tasks.FindRequest.Types.Response Tasks { get; set; }
        
        private ListItem SelectedList { get; set; }
        private TaskItem SelectedItem { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Lists = await ListApi.FindAsync(new Api.Contract.Lists.FindRequest
            {

            });
        }

        private async Task ListRowSelected(ListItem arg)
        {
            SelectedList = arg;
            
            await ReloadTasks(arg.Id);
        }

        private void ListRowDeselected(ListItem arg)
        {
            SelectedList = null;
            Tasks = new FindRequest.Types.Response();
        }

        private async Task ReloadTasks(string listId)
        {
            Tasks = await TaskApi.FindAsync(new FindRequest
            {
                ListId = listId,
                
            });
        }

        private void OnListAdd(MouseEventArgs obj)
        {
            SelectedList = new ListItem();
        }

        private async Task OnListSaved(MouseEventArgs arg)
        {
            if (SelectedList.Id is {Length: > 0})
            {
                await ListApi.RenameAsync(new RenameRequest
                {
                    Id = SelectedList.Id, 
                    Name = SelectedList.Name,
                });
            }
            else
            {
                await ListApi.AddAsync(new AddRequest()
                {
                    Name = SelectedList.Name
                });
            }
        }

        private void OnItemAdd(MouseEventArgs obj)
        {
            SelectedItem = new TaskItem();
        }
        
        private async Task OnItemSaved(MouseEventArgs obj)
        {
            await TaskApi.AddAsync(new Api.Contract.Tasks.AddRequest
            {
                ListId = SelectedList.Id, 
                Title = SelectedItem.Title,
            });
        }
    }
}