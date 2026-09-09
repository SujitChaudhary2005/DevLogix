namespace DevLogix.Services.Notifications;

public sealed class EmailNotification : NotificationBase
{
    public override Task SendAsync()
    {
        base.LogAttempt("Email");
        Console.WriteLine($"Email would be sent to {RecipientId}");
        return Task.CompletedTask;
    }
}
