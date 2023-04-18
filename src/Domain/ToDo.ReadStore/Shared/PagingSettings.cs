using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ToDo.ReadStore.Shared;

public class FilterSettingsCollection<TModel> : HashSet<Expression<Func<TModel, bool>>>
{
}

public class SortingSettingsCollection<TModel> : HashSet<SortingSettings<TModel>>
{
}

public class SortingSettings<TModel>
{
    public Expression<Func<TModel, object>> Expression { get; set; }
    public bool Descending { get; set; }
}

public class PagingSettings
{
    public ushort Page { get; set; } = 1;
    public ushort Rows { get; set; } = 10;
}