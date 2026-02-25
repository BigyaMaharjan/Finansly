using Finansly.Application.DTOs.Reports;

namespace Finansly.Application.Services.Reports;

public interface IReportService
{
    Task<MonthlySummaryDto> GetMonthlySummaryAsync(Guid userId, int month, int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync(Guid userId, int month, int year, CancellationToken cancellationToken = default);
    Task<MonthlySummaryDto> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
}
