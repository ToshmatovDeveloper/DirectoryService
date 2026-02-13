using DirectoryService.Application.Abstractions;

namespace Shared;

public record PaginationResponse<T>(IReadOnlyCollection<T> Items, int Page, int TotalCount)
    : IQueryHandler.IQuery;