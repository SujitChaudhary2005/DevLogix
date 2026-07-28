namespace DevLogix.Models;

public class TimeLog
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int MinutesSpent { get; set; }
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}