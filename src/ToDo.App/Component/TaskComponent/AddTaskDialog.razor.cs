using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using ToDo.Api.Contract.Tasks;
using ToDo.App.Helpers.UI;

namespace ToDo.App.Component.TaskComponent;

public partial class AddTaskDialog
{
    [Inject] public TasksService.TasksServiceClient TasksApi { get; private set; }
    [Inject] public DialogService DialogService { get; private set; }
    
    [Parameter] public string ListId { get; set; }
    [Parameter] public EventCallback<string> OnTaskCreated { get; set; }

    private AddTaskModel Model = new();

    private class AddTaskModel
    {
        public string Name { get; set; }
    }

    private async Task AddTask(MouseEventArgs arg)
    {
        var result = await TasksApi.AddAsync(new AddRequest
        {
            ListId = ListId,
            Title = Model.Name
        });

        if (OnTaskCreated is {})
        {
            DialogService.Close();
            await OnTaskCreated.InvokeAsync(result.TaskId);
        }
    }
    
    public sealed class DialogParams : DialogParameters
    {
        public DialogParams WithListId(string listId)
        {
            Add(nameof(AddTaskDialog.ListId), listId);
            
            return this;
        }

        public DialogParams WithOnTaskCreated(EventCallback<string> callback)
        {
            Add(nameof(OnTaskCreated), callback);
            
            return this;
        }
    }
}