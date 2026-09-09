namespace DevLogix.Services.Notifications;

using DevLogix.Models;

public interface INotificationDispatcher
{
    event TicketStatusChangedHandler? OnTicketStatusChanged;
    Task NotifyStatusChangeAsync(int ticketId, TicketStatus oldStatus, TicketStatus newStatus, string changedByUserId);
}
