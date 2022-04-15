using System;
using System.Threading.Tasks;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;

namespace ToDo.App.Pages.List;

public partial class List
{
    [Inject] protected ListViewModel Model { get; set; }
    
    private async Task OnReadData(DataGridReadDataEventArgs<ListViewModel.ListItemModel> arg)
    {
        await Model.Reload(null, arg.Page, arg.PageSize);
    }

    private void OnSelectedRowChanged(ListViewModel.ListItemModel arg)
    {
        Model.SelectedList = arg;
    }
}