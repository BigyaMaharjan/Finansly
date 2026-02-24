using Finansly.Application.DTOs.Reports;
using Finansly.Application.Interfaces.Reports;
using Finansly.Application.Services.Reports;

namespace Finansly.Infrastructure.Services.Reports;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;

    public ReportService(IReportRepository repository)
    {
        _repository = repository;
    }

    public Task<MonthlySummaryDto> GetMonthlySummaryAsync(Guid userId, int month, int year)
        => _repository.GetMonthlySummaryAsync(userId, month, year);

    public Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync(Guid userId, int month, int year)
        => _repository.GetCategoryBreakdownAsync(userId, month, year);

    public Task<MonthlySummaryDto> GetBalanceAsync(Guid userId)
        => _repository.GetBalanceAsync(userId);
}
