using System.Collections.Generic;

namespace ToDo.ReadStore.Shared;

public class PagedResult<TItem>
{
    public int TotalRows { get; protected set; }
    public IReadOnlyCollection<TItem> Result { get; protected set; }
}