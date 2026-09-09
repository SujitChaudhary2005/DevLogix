namespace DevLogix.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}