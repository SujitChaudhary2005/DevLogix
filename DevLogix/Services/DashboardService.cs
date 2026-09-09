namespace DevLogix.Services;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DashboardService : IDashboardService
{
    private readonly string _connectionString;

    public DashboardService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");
    }

    public async Task<DashboardMetrics> GetMetricsAsync()
    {
        var metrics = new DashboardMetrics();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // 1. Basic counts
        var basicCountsQuery = @"
            SELECT 
                (SELECT COUNT(*) FROM Projects) as TotalProjects,
                (SELECT COUNT(*) FROM Tickets) as TotalTickets,
                (SELECT COUNT(*) FROM AspNetUsers) as TotalUsers,
                (SELECT COUNT(*) FROM Tickets WHERE Status != 3 AND Status != 4) as OpenTickets,
                (SELECT COUNT(*) FROM Tickets WHERE Status = 2) as ResolvedTickets
        ";

        using (var command = new SqlCommand(basicCountsQuery, connection))
        {
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                metrics.TotalProjects = reader.GetInt32(0);
                metrics.TotalTickets = reader.GetInt32(1);
                metrics.TotalUsers = reader.GetInt32(2);
                metrics.OpenTickets = reader.GetInt32(3);
                metrics.ResolvedTickets = reader.GetInt32(4);
            }
        }

        // 2. Status counts
        var statusQuery = "SELECT Status, COUNT(*) as Count FROM Tickets GROUP BY Status";
        using (var command = new SqlCommand(statusQuery, connection))
        {
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var statusInt = reader.GetInt32(0);
                // Simple mapping, normally we'd parse the enum
                var statusStr = statusInt switch
                {
                    0 => "Todo",
                    1 => "InProgress",
                    2 => "Resolved",
                    3 => "Closed",
                    4 => "Reopened",
                    _ => "Unknown"
                };
                
                metrics.TicketsByStatus.Add(new StatusCount
                {
                    Status = statusStr,
                    Count = reader.GetInt32(1)
                });
            }
        }

        // 3. Priority counts
        var priorityQuery = "SELECT Priority, COUNT(*) as Count FROM Tickets GROUP BY Priority";
        using (var command = new SqlCommand(priorityQuery, connection))
        {
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var priorityInt = reader.GetInt32(0);
                var priorityStr = priorityInt switch
                {
                    0 => "Low",
                    1 => "Medium",
                    2 => "High",
                    3 => "Critical",
                    _ => "Unknown"
                };

                metrics.TicketsByPriority.Add(new PriorityCount
                {
                    Priority = priorityStr,
                    Count = reader.GetInt32(1)
                });
            }
        }

        // 4. Comments and Time
        // Parameterized query just to show ADO.NET usage, even though parameter is hardcoded here
        var sumQuery = @"
            SELECT 
                (SELECT COUNT(*) FROM Comments WHERE 1=@alwaysTrue) as TotalComments,
                (SELECT ISNULL(SUM(MinutesSpent), 0) FROM TimeLogs WHERE 1=@alwaysTrue) as TotalTime
        ";
        using (var command = new SqlCommand(sumQuery, connection))
        {
            command.Parameters.AddWithValue("@alwaysTrue", 1);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                metrics.TotalComments = reader.GetInt32(0);
                metrics.TotalTimeLoggedMinutes = reader.GetInt32(1);
            }
        }

        return metrics;
    }
}
