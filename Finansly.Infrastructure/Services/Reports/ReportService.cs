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

    public Task<MonthlySummaryDto> GetMonthlySummaryAsync(Guid userId, int month, int year, CancellationToken cancellationToken = default)
        => _repository.GetMonthlySummaryAsync(userId, month, year, cancellationToken);

    public Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync(Guid userId, int month, int year, CancellationToken cancellationToken = default)
        => _repository.GetCategoryBreakdownAsync(userId, month, year, cancellationToken);

    public Task<MonthlySummaryDto> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
        => _repository.GetBalanceAsync(userId, cancellationToken);
}
