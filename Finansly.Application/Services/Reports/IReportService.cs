using Finansly.Application.DTOs.Reports;

namespace Finansly.Application.Services.Reports;

public interface IReportService
{
    Task<MonthlySummaryDto> GetMonthlySummaryAsync(Guid userId, int month, int year);
    Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync(Guid userId, int month, int year);
    Task<MonthlySummaryDto> GetBalanceAsync(Guid userId);
}
