namespace DevLogix.Services.Notifications;

using DevLogix.Data;
using DevLogix.Models;

public sealed class InAppNotification : NotificationBase
{
    private readonly ApplicationDbContext _context;

    public InAppNotification(ApplicationDbContext context)
    {
        _context = context;
    }

    public int? RelatedTicketId { get; set; }

    public override async Task SendAsync()
    {
        base.LogAttempt("InApp");

        var notification = new Notification
        {
            RecipientUserId = RecipientId,
            Channel = NotificationChannel.InApp,
            Message = Message,
            RelatedTicketId = RelatedTicketId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}
