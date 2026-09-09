namespace DevLogix.ViewModels.Projects;

public class ProjectListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public int MemberCount { get; set; }
    public int TicketCount { get; set; }
}
