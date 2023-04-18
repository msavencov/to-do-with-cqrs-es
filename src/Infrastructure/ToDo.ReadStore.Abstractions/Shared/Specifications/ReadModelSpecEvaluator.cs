using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ToDo.ReadStore.Abstractions.Shared.Specifications;

public static class ReadModelSpecEvaluator
{
    public static async Task<(int TotalRows, IQueryable<TModel> Result)> ApplySpecification<TModel>(
        this IQueryable<TModel> source, ReadModelSpec<TModel> spec, CancellationToken ct = default)
    {
        if (spec.Criteria is { })
        {
            source = source.Where(spec.Criteria.Predicate);
        }

        var rows = await source.CountAsync(ct);

        if (spec.OrderBy is { } orderBy)
        {
            source = orderBy.Direction switch
            {
                SortDirection.Ascending => source.OrderBy(orderBy.Predicate),
                SortDirection.Descending => source.OrderByDescending(orderBy.Predicate),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (spec.Paging is { })
        {
            source = source.Skip(spec.Paging.Skip).Take(spec.Paging.Take);
        }

        return (rows, source);
    }
}