using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net.Sockets;
using System.Xml.Linq;

namespace DevLogix.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
    public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Attachment> UploadedAttachments { get; set; } = new List<Attachment>();
}