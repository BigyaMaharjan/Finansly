using Finansly.Application.DTOs.Reports;

namespace Finansly.Application.Services.Reports;

public interface IReportService
{
    Task<MonthlySummaryDto> GetMonthlySummaryAsync(Guid userId, MonthYearRequestDto request, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync(Guid userId, MonthYearRequestDto request, CancellationToken cancellationToken = default);
    Task<MonthlySummaryDto> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
}
