using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using ToDo.Api.Contract.Lists;
using ToDo.Api.Contract.Shared;

namespace ToDo.App.Component.ListComponent;

public partial class ToDoListGrid
{
    [Inject] public ListService.ListServiceClient ListApi { get; private set; }
    [Inject] public DialogService DialogService { get; private set; }

    [Parameter] public EventCallback<GridRow> OnListItemSelected { get; set; }

    private RadzenDataGrid<GridRow> grid;
    private bool isLoading = false;
    private int count;
    private IEnumerable<GridRow> listResult;

    private async Task Reset()
    {
        grid.Reset();
        await grid.FirstPage(true);
    }

    private async Task LoadData(LoadDataArgs arg)
    {
        var take = arg.Top ?? 10;
        var page = (arg.Skip ?? 0) / take + 1;

        isLoading = true;

        var apiResult = await ListApi.FindAsync(new FindRequest
        {
            Criteria = arg.Filter,
            OrderBy = arg.OrderBy,
            Paging = new Paging
            {
                Page = page,
                Size = take,
            }
        });

        listResult = apiResult?.Result.Select(t => new GridRow
        {
            Id = t.Id,
            Name = t.Name, 
            CreatedAt = t.CreatedAt.ToDateTimeOffset(),
            CreatedBy = t.CreatedBy,
            ActiveCount = t.ActiveCount,
            CompletedCount = t.CompletedCount,
        });
        count = apiResult?.Page.TotalRows ?? 0;
        isLoading = false;
    }
    
    private async Task EditDialogOpen(MouseEventArgs arg)
    {
        var options = new DialogOptions
        {
            ShowClose = false,
        };

        await DialogService.OpenAsync<ToDoListEdit>("Add ToDo List", options: options);
    }
    private class Filter
    {
        public string Name { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }

    public sealed class GridRow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public int CompletedCount { get; set; }
        public int ActiveCount { get; set; }
    }
}