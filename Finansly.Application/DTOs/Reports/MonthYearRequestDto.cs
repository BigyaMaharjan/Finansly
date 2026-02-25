namespace Finansly.Application.DTOs.Reports;

public record MonthYearRequestDto
{
    public int Month { get; init; }
    public int Year { get; init; }
}
