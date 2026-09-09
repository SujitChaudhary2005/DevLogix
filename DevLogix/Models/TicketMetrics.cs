namespace DevLogix.Models;

public readonly struct TicketMetrics
{
    public int Total { get; init; }
    public int Open { get; init; }
    public int Resolved { get; init; }
    public int Closed { get; init; }
    public double AverageResolutionMinutes { get; init; }

    public double OpenPercentage => Total == 0 ? 0 : (double)Open / Total * 100;
    public double ResolvedPercentage => Total == 0 ? 0 : (double)Resolved / Total * 100;
}
