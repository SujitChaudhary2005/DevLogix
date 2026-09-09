namespace DevLogix.Services;

public interface IDashboardService
{
    Task<DashboardMetrics> GetMetricsAsync();
}

public class DashboardMetrics
{
    public int TotalProjects { get; set; }
    public int TotalTickets { get; set; }
    public int OpenTickets { get; set; }
    public int ResolvedTickets { get; set; }
    public int TotalUsers { get; set; }
    public int TotalComments { get; set; }
    public int TotalTimeLoggedMinutes { get; set; }
    public List<StatusCount> TicketsByStatus { get; set; } = new();
    public List<PriorityCount> TicketsByPriority { get; set; } = new();
}

public class StatusCount
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class PriorityCount
{
    public string Priority { get; set; } = string.Empty;
    public int Count { get; set; }
}
