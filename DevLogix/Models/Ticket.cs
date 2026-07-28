using System.Net.Mail;
using System.Xml.Linq;

namespace DevLogix.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.Todo;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string CreatedById { get; set; } = string.Empty;
    public ApplicationUser CreatedBy { get; set; } = null!;

    public string? AssignedToId { get; set; }
    public ApplicationUser? AssignedTo { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}