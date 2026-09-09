namespace DevLogix.ViewModels.Tickets;

using DevLogix.Models;
using System;
using System.Collections.Generic;

public class TicketListViewModel
{
    public List<TicketItemViewModel> Tickets { get; set; } = new();
    public TicketStatus? StatusFilter { get; set; }
    public Priority? PriorityFilter { get; set; }
    public string? AssigneeFilter { get; set; }
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
}

public class TicketItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public Priority Priority { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public string? AssignedToName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CommentCount { get; set; }
}
