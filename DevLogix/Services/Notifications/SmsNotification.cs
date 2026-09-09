namespace DevLogix.Services.Notifications;

public sealed class SmsNotification : NotificationBase
{
    public override Task SendAsync()
    {
        base.LogAttempt("SMS");
        Console.WriteLine($"SMS would be sent to {RecipientId}");
        return Task.CompletedTask;
    }
}
