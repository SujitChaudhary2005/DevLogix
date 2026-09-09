namespace DevLogix.ViewModels.Projects;

using DevLogix.Models;
using System;
using System.Collections.Generic;

public class ProjectDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public List<MemberViewModel> Members { get; set; } = new();
    public List<TicketSummaryViewModel> RecentTickets { get; set; } = new();
    public TicketMetrics Metrics { get; set; }
}

public class MemberViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
}

public class TicketSummaryViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public Priority Priority { get; set; }
    public string? AssignedToName { get; set; }
    public DateTime CreatedAt { get; set; }
}
