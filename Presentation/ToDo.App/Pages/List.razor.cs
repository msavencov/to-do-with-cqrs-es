using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDo.Api.Contract.ToDo;
using ToDo.Api.Contract.ToDo.Models;

namespace ToDo.App.Pages
{
    public partial class List
    {
        [Inject] public IToDo Api { get; private set; }
        [Inject] public NavigationManager NavigationManager { get; private set; }
        
        private ToDoListCollection Lists { get; set; }
        private ToDoItemCollection Tasks { get; set; }
        
        private ToDoList SelectedList { get; set; }
        private ToDoItem SelectedItem { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Lists = await Api.GetAllLists();
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
            Tasks = await Api.GetTasks(listId);
        }

        private void OnListAdd(MouseEventArgs obj)
        {
            SelectedList = new ToDoList();
        }

        private async Task OnListSaved(MouseEventArgs arg)
        {
            if (SelectedList.Id is not null)
            {
                await Api.UpdateListName(SelectedList.Id, SelectedList.Name);
            }
            else
            {
                await Api.NewList(SelectedList.Name);
            }
        }

        private void OnItemAdd(MouseEventArgs obj)
        {
            SelectedItem = new ToDoItem();
        }
        
        private async Task OnItemSaved(MouseEventArgs obj)
        {
            await Api.NewTask(SelectedList.Id, SelectedItem.Task);
        }
    }
}