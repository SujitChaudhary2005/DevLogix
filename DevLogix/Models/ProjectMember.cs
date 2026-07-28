namespace DevLogix.Models;

public class ProjectMember
{
    public int Id { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}