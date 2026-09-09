namespace DevLogix.Services.Notifications;

using DevLogix.Data;
using DevLogix.Models;

// Explicit custom delegate (syllabus requirement)
public delegate void TicketStatusChangedHandler(int ticketId, TicketStatus oldStatus, TicketStatus newStatus, string changedByUserId);

public class NotificationDispatcher : INotificationDispatcher
{
    private readonly ApplicationDbContext _context;

    public NotificationDispatcher(ApplicationDbContext context)
    {
        _context = context;
    }

    // Event using the explicit delegate
    public event TicketStatusChangedHandler? OnTicketStatusChanged;

    public async Task NotifyStatusChangeAsync(int ticketId, TicketStatus oldStatus, TicketStatus newStatus, string changedByUserId)
    {
        // Fire the event
        OnTicketStatusChanged?.Invoke(ticketId, oldStatus, newStatus, changedByUserId);

        // Polymorphic dispatch — send InApp notification to ticket creator/assignee
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null) return;

        var message = $"Ticket '{ticket.Title}' status changed from {oldStatus} to {newStatus}";

        // Send to ticket creator (if different from person who changed it)
        if (ticket.CreatedById != changedByUserId)
        {
            NotificationBase notification = new InAppNotification(_context)
            {
                RecipientId = ticket.CreatedById,
                Message = message,
                RelatedTicketId = ticketId
            };
            await notification.SendAsync();
        }

        // Send to assignee (if exists and different from changer)
        if (ticket.AssignedToId != null && ticket.AssignedToId != changedByUserId)
        {
            NotificationBase notification = new InAppNotification(_context)
            {
                RecipientId = ticket.AssignedToId,
                Message = message,
                RelatedTicketId = ticketId
            };
            await notification.SendAsync();
        }
    }
}
