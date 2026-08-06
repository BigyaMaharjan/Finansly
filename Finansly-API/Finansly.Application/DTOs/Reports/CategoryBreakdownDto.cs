namespace Finansly.Application.DTOs.Reports;

public record CategoryBreakdownDto
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public string Type { get; init; } = null!;
    public decimal Total { get; init; }
}
