namespace DevLogix.Models;

public enum TicketStatus
{
    Todo,
    InProgress,
    Resolved,
    Closed,
    Reopened
}

public enum Priority
{
    Low,
    Medium,
    High,
    Critical
}

public enum NotificationChannel
{
    Email,
    InApp,
    Sms
}