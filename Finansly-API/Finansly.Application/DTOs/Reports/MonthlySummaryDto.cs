namespace Finansly.Application.DTOs.Reports;

public record MonthlySummaryDto
{
    public decimal Income { get; init; }
    public decimal Expense { get; init; }
    public decimal Balance => Income - Expense;
    public int? Month { get; init; }
    public int? Year { get; init; }
}
