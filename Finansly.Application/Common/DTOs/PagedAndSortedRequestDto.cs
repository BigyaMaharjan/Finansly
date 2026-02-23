using Finansly.Application.Common.Enums;

namespace Finansly.Application.Common.DTOs;

public record PagedAndSortedRequestDto
{
    public int SkipCount { get; init; } = 0;
    public int MaxResultCount { get; init; } = 10;
    public string? Sorting { get; init; }
    public SortType SortType { get; init; } = SortType.Descending;
}
