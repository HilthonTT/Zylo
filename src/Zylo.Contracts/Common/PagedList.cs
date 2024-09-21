﻿using Microsoft.EntityFrameworkCore;

namespace Zylo.Contracts.Common;

public sealed class PagedList<T>
{
    private const int MaxPageSize = 100;

    private PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; }

    public int Page { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public bool HasNextPage => Page * PageSize < TotalCount;

    public bool HasPreviousPage => Page > 1;

    public static async Task<PagedList<T>> CreateAsync(
        IQueryable<T> query, 
        int page, 
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        int totalCount = await query.CountAsync(cancellationToken);

        pageSize = Math.Min(Math.Max(pageSize, 0), MaxPageSize);

        List<T> items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new(items, page, pageSize, totalCount);
    }
}
