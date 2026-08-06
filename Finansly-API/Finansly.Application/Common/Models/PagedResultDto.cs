namespace Finansly.Application.Common.Models;

public record PagedResultDto<T>
{
    public List<T> Items { get; init; } = [];
    public long TotalCount { get; init; }
}
