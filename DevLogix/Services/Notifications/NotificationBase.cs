namespace DevLogix.Services.Notifications;

public abstract class NotificationBase
{
    public string RecipientId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    // Demonstrates `base` keyword — subclasses call base.LogAttempt(...)
    protected virtual void LogAttempt(string channel)
    {
        Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Attempting {channel} notification to {RecipientId}: {Message}");
    }

    public abstract Task SendAsync();
}
