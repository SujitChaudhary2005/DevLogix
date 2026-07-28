namespace DevLogix.Models;

public class Notification
{
    public int Id { get; set; }

    public string RecipientUserId { get; set; } = string.Empty;
    public ApplicationUser RecipientUser { get; set; } = null!;

    public NotificationChannel Channel { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    public int? RelatedTicketId { get; set; }
    public Ticket? RelatedTicket { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}