using Finansly.Domain.Enums;

namespace Finansly.Application.DTOs.Transactions;

public record GetTotalByTypeRequestDto
{
    public Guid UserId { get; init; }
    public CategoryType Type { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
}