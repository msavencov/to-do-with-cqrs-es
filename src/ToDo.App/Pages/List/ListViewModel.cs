using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using ToDo.Api.Contract.Lists;
using ToDo.Api.Contract.Shared;

namespace ToDo.App.Pages.List;

public class ListViewModel
{
    private readonly ListService.ListServiceClient ListApi;

    public ListViewModel(ListService.ListServiceClient listApi)
    {
        ListApi = listApi;
    }
    
    
    public class ListItemModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int ActiveCount { get; set; }
        public int CompletedCount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public IEnumerable<ListItemModel> Lists { get; private set; } = Enumerable.Empty<ListItemModel>();
    public ListItemModel SelectedList { get; set; }

    public async Task Reload(string search, int page, int pageSize)
    {
        var request = new FindRequest
        {
            Paging = new Paging
            {
                Page = page, Size = pageSize
            }
        };
        
        var response = await ListApi.FindAsync(request);
        var items = response.Result.Select(t => new ListItemModel
        {
            Id = t.Id,
            Name = t.Name,
            ActiveCount = t.ActiveCount,
            CompletedCount = t.CompletedCount,
            CreatedAt = t.CreatedAt.ToDateTime(),
            CreatedBy = t.CreatedBy,
        });

        Lists = items;
    }
}