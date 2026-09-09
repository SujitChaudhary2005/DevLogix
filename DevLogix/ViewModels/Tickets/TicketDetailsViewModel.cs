namespace DevLogix.ViewModels.Tickets;

using DevLogix.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

public class TicketDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public string CreatedByName { get; set; } = string.Empty;
    public string? AssignedToName { get; set; }
    public string? AssignedToId { get; set; }

    public List<CommentViewModel> Comments { get; set; } = new();
    public List<TimeLogViewModel> TimeLogs { get; set; } = new();
    public List<AttachmentViewModel> Attachments { get; set; } = new();

    // For inline forms
    public string NewCommentContent { get; set; } = string.Empty;
    public int NewTimeLogMinutes { get; set; }
    public string? NewTimeLogNote { get; set; }
    public SelectList? ProjectMembers { get; set; }
}

public class CommentViewModel
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool CanDelete { get; set; }
}

public class TimeLogViewModel
{
    public int Id { get; set; }
    public int MinutesSpent { get; set; }
    public string? Note { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; }
}

public class AttachmentViewModel
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTime UploadedAt { get; set; }
    public string UploadedByName { get; set; } = string.Empty;
}
