using System.Collections.Generic;
using System.Linq;

namespace ToDo.ReadStore.Abstractions.Shared;

public class PagedResult<TItem>
{
    public PagedResult(int totalRows, IEnumerable<TItem> result)
    {
        TotalRows = totalRows;
        Result = result as IReadOnlyCollection<TItem> ?? result.ToList();
    }
    public int TotalRows { get; }
    public IReadOnlyCollection<TItem> Result { get; }
}