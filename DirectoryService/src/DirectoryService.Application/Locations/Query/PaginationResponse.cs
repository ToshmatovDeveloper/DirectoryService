using DirectoryService.Application.Abstractions;

namespace Shared;

public class PaginationResponse<T> : IQueryHandler.IQuery
{
    public IReadOnlyCollection<T> Items { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public PaginationResponse(
        IEnumerable<T> items,
        int page,
        int pageSize,
        int totalCount)
    {
        Items = items.ToList().AsReadOnly();
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
