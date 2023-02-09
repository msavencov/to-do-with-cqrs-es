using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using ToDo.Api.Contract.Lists;

namespace ToDo.App.Component.ListComponent;

public partial class ToDoListEdit
{
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private ListService.ListServiceClient Api { get; set; }

    public ListItem SelectedItem;

    protected override void OnParametersSet()
    {
        SelectedItem = new ListItem();
    }

    private async Task SaveList(MouseEventArgs arg)
    {
        await Api.AddAsync(new AddRequest
        {
            Name = SelectedItem.Name,
        });

        DialogService.Close();
    }
}