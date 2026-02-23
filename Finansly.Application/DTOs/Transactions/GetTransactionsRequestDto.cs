using Finansly.Application.Common.DTOs;
using Finansly.Domain.Enums;

namespace Finansly.Application.DTOs.Transactions;

public record GetTransactionsRequestDto : PagedAndSortedRequestDto
{
    public Guid? CategoryId { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public CategoryType? Type { get; init; }
}
