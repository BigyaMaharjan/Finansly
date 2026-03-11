using Dapper;
using Finansly.Application.DTOs.Reports;
using Finansly.Application.Interfaces.Reports;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Finansly.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly string _connectionString;

    public ReportRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(Guid userId, int month, int year, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                COALESCE(SUM(CASE WHEN c."Type" = 1 THEN t."Amount" ELSE 0 END), 0) AS "Income",
                COALESCE(SUM(CASE WHEN c."Type" = 2 THEN t."Amount" ELSE 0 END), 0) AS "Expense"
            FROM "Transactions" t
            INNER JOIN "Categories" c ON t."CategoryId" = c."Id"
            WHERE t."UserId" = @UserId
              AND EXTRACT(MONTH FROM t."Date") = @Month
              AND EXTRACT(YEAR  FROM t."Date") = @Year
              AND t."IsDeleted" = false
              AND c."IsDeleted" = false
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        var cmd = new CommandDefinition(sql, new { UserId = userId, Month = month, Year = year }, cancellationToken: cancellationToken);
        var row = await conn.QuerySingleAsync<(decimal Income, decimal Expense)>(cmd);

        return new MonthlySummaryDto
        {
            Income = row.Income,
            Expense = row.Expense,
            Month = month,
            Year = year
        };
    }

    public async Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync(Guid userId, int month, int year, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                c."Id"   AS "CategoryId",
                c."Name" AS "CategoryName",
                CASE c."Type" WHEN 1 THEN 'Income' ELSE 'Expense' END AS "Type",
                COALESCE(SUM(t."Amount"), 0) AS "Total"
            FROM "Categories" c
            LEFT JOIN "Transactions" t
                   ON t."CategoryId" = c."Id"
                  AND EXTRACT(MONTH FROM t."Date") = @Month
                  AND EXTRACT(YEAR  FROM t."Date") = @Year
                  AND t."IsDeleted" = false
            WHERE c."UserId" = @UserId
              AND c."IsDeleted" = false
            GROUP BY c."Id", c."Name", c."Type"
            ORDER BY "Total" DESC
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        var cmd = new CommandDefinition(sql, new { UserId = userId, Month = month, Year = year }, cancellationToken: cancellationToken);
        return await conn.QueryAsync<CategoryBreakdownDto>(cmd);
    }

    public async Task<MonthlySummaryDto> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                COALESCE(SUM(CASE WHEN c."Type" = 1 THEN t."Amount" ELSE 0 END), 0) AS "Income",
                COALESCE(SUM(CASE WHEN c."Type" = 2 THEN t."Amount" ELSE 0 END), 0) AS "Expense"
            FROM "Transactions" t
            INNER JOIN "Categories" c ON t."CategoryId" = c."Id"
            WHERE t."UserId" = @UserId
              AND t."IsDeleted" = false
              AND c."IsDeleted" = false
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        var cmd = new CommandDefinition(sql, new { UserId = userId }, cancellationToken: cancellationToken);
        var row = await conn.QuerySingleAsync<(decimal Income, decimal Expense)>(cmd);

        return new MonthlySummaryDto
        {
            Income = row.Income,
            Expense = row.Expense
        };
    }
}
