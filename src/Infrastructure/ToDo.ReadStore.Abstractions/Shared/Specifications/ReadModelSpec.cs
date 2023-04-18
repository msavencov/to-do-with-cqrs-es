using System;
using System.Linq.Expressions;

namespace ToDo.ReadStore.Abstractions.Shared.Specifications;

public class ReadModelSpec<TModel>
{
    public FilterSettings Criteria { get; private set; }
    public SortSettings OrderBy { get; private set; }
    public PageSettings Paging { get; private set; }

    public ReadModelSpec<TModel> WithCriteria(Expression<Func<TModel, bool>> predicate)
    {
        Criteria = new FilterSettings(predicate);
        return this;
    }

    public ReadModelSpec<TModel> SortBy(Expression<Func<TModel, object>> orderBy, bool descending = false)
    {
        OrderBy = new SortSettings(orderBy, descending ? SortDirection.Descending : SortDirection.Ascending);
        return this;
    }

    public ReadModelSpec<TModel> Paged(ushort page, ushort rows)
    {
        if (page < 1) throw new ArgumentException("The page number must be greater than 0.");

        Paging = new PageSettings(page, rows);

        return this;
    }

    public sealed class FilterSettings
    {
        public Expression<Func<TModel, bool>> Predicate { get; }

        public FilterSettings(Expression<Func<TModel,bool>> predicate)
        {
            Predicate = predicate;
        }
    }

    public sealed class SortSettings
    {
        public Expression<Func<TModel, object>> Predicate { get; }
        public SortDirection Direction { get; }

        public SortSettings(Expression<Func<TModel, object>> orderBy, SortDirection direction)
        {
            Predicate = orderBy;
            Direction = direction;
        }
    }
    
    public sealed class PageSettings
    {
        public PageSettings(ushort page, ushort rows)
        {
            Take = rows;
            Skip = (ushort)((page - 1) * rows);
        }

        public ushort Take { get; }
        public ushort Skip { get; }
    }
}

public enum SortDirection
{
    Ascending,
    Descending,
}
