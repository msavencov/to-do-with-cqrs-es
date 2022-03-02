using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDo.Api.Client.ToDoApi.Services;
using ToDo.Api.Contract.List.Models;
using ToDo.Api.Contract.List.Operations;
using ToDo.Api.Contract.Tasks.Models;
using ToDo.Api.Contract.Tasks.Operations;
using ToDo.App.Extensions;
using ToDo.App.Shared;

namespace ToDo.App.Pages
{
    public partial class List
    {
        [Inject] public IListApi ListApi { get; private set; }
        [Inject] public IToDoApi TaskApi { get; private set; }
        [Inject] public NavigationManager NavigationManager { get; private set; }
        
        [CascadingParameter] public Error Error { get; set; }
        
        private ToDoListCollection Lists { get; set; }
        private ToDoItemCollection Tasks { get; set; }
        
        private ToDoList SelectedList { get; set; }
        private ToDoItem SelectedItem { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Lists = await ListApi.CallSafeAsync(t => t.FindListsAsync(new FindLists()), exception =>
            {
                Error.ProcessError(exception);
            }) ?? new ToDoListCollection();
        }

        private async Task ListRowSelected(ToDoList arg)
        {
            SelectedList = arg;
            
            await ReloadTasks(arg.Id);
        }

        private void ListRowDeselected(ToDoList arg)
        {
            SelectedList = null;
            Tasks = new ToDoItemCollection();
        }

        private async Task ReloadTasks(string listId)
        {
            Tasks = await ListApi.GetTasksAsync(new GetTasks
            {
                ListId = listId
            });
        }

        private void OnListAdd(MouseEventArgs obj)
        {
            SelectedList = new ToDoList();
        }

        private async Task OnListSaved(MouseEventArgs arg)
        {
            if (SelectedList.Id is not null)
            {
                await ListApi.RenameListAsync(new RenameList
                {
                    Id = SelectedList.Id, 
                    Name = SelectedList.Name,
                });
            }
            else
            {
                await ListApi.CreateListAsync(new CreateList
                {
                    Name = SelectedList.Name
                });
            }
        }

        private void OnItemAdd(MouseEventArgs obj)
        {
            SelectedItem = new ToDoItem();
        }
        
        private async Task OnItemSaved(MouseEventArgs obj)
        {
            await TaskApi.AddTaskAsync(new AddTask
            {
                ListId = SelectedList.Id,
                Task = SelectedItem.Task,
            });
        }
    }
}