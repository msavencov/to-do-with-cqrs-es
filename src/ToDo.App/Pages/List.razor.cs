using ToDo.App.Component.ListComponent;

namespace ToDo.App.Pages;

public partial class List
{
    private string ListId;
    
    private void OnListSelected(ToDoListGrid.GridRow arg)
    {
        ListId = arg.Id;
    }
}