namespace DevLogix.Services;

using DevLogix.Data;
using DevLogix.Exceptions;
using DevLogix.Models;
using DevLogix.ViewModels.Tickets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;
    private readonly IProjectService _projectService;

    public TicketService(ApplicationDbContext context, IProjectService projectService)
    {
        _context = context;
        _projectService = projectService;
    }

    public async Task<TicketListViewModel> GetTicketsAsync(int projectId, string userId, TicketStatus? status, Priority? priority, string? assignee, int page, int pageSize)
    {
        if (!await _projectService.IsProjectMemberAsync(projectId, userId))
            throw new ForbiddenException("You do not have access to this project.");

        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            throw new NotFoundException(nameof(Project), projectId);

        var query = _context.Tickets
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .Include(t => t.Comments)
            .Where(t => t.ProjectId == projectId)
            .AsQueryable();

        if (status.HasValue) query = query.Where(t => t.Status == status.Value);
        if (priority.HasValue) query = query.Where(t => t.Priority == priority.Value);
        if (!string.IsNullOrEmpty(assignee)) query = query.Where(t => t.AssignedToId == assignee);

        int totalTickets = await query.CountAsync();
        var tickets = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new TicketListViewModel
        {
            ProjectId = projectId,
            ProjectTitle = project.Title,
            StatusFilter = status,
            PriorityFilter = priority,
            AssigneeFilter = assignee,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalTickets / (double)pageSize),
            Tickets = tickets.Select(t => new TicketItemViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                Priority = t.Priority,
                CreatedByName = t.CreatedBy?.FullName ?? "Unknown",
                AssignedToName = t.AssignedTo?.FullName,
                CreatedAt = t.CreatedAt,
                CommentCount = t.Comments.Count
            }).ToList()
        };
    }

    public async Task<TicketDetailsViewModel> GetTicketDetailsAsync(int id, string userId)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Project)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .Include(t => t.Comments)
                .ThenInclude(c => c.User)
            .Include(t => t.TimeLogs)
                .ThenInclude(tl => tl.User)
            .Include(t => t.Attachments)
                .ThenInclude(a => a.UploadedBy)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new NotFoundException(nameof(Ticket), id);

        if (!await _projectService.IsProjectMemberAsync(ticket.ProjectId, userId))
            throw new ForbiddenException("You do not have access to this ticket.");

        return new TicketDetailsViewModel
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            CreatedAt = ticket.CreatedAt,
            ProjectId = ticket.ProjectId,
            ProjectTitle = ticket.Project?.Title ?? "Unknown",
            CreatedByName = ticket.CreatedBy?.FullName ?? "Unknown",
            AssignedToName = ticket.AssignedTo?.FullName,
            AssignedToId = ticket.AssignedToId,
            Comments = ticket.Comments.OrderBy(c => c.CreatedAt).Select(c => new CommentViewModel
            {
                Id = c.Id,
                Content = c.Content,
                UserName = c.User?.FullName ?? "Unknown",
                CreatedAt = c.CreatedAt,
                CanDelete = c.UserId == userId // Or admin
            }).ToList(),
            TimeLogs = ticket.TimeLogs.OrderByDescending(tl => tl.LoggedAt).Select(tl => new TimeLogViewModel
            {
                Id = tl.Id,
                MinutesSpent = tl.MinutesSpent,
                Note = tl.Note,
                UserName = tl.User?.FullName ?? "Unknown",
                LoggedAt = tl.LoggedAt
            }).ToList(),
            Attachments = ticket.Attachments.OrderByDescending(a => a.UploadedAt).Select(a => new AttachmentViewModel
            {
                Id = a.Id,
                FileName = a.FileName,
                ContentType = a.ContentType,
                FileSizeBytes = a.FileSizeBytes,
                UploadedAt = a.UploadedAt,
                UploadedByName = a.UploadedBy?.FullName ?? "Unknown"
            }).ToList()
        };
    }

    public async Task<int> CreateTicketAsync(TicketCreateViewModel model, string userId)
    {
        if (!await _projectService.IsProjectMemberAsync(model.ProjectId, userId))
            throw new ForbiddenException("You do not have access to create tickets in this project.");

        var ticket = new Ticket
        {
            ProjectId = model.ProjectId,
            Title = model.Title,
            Description = model.Description,
            Priority = model.Priority,
            Status = TicketStatus.Todo,
            CreatedById = userId,
            AssignedToId = model.AssignedToId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return ticket.Id;
    }

    public async Task UpdateTicketAsync(TicketEditViewModel model, string userId)
    {
        var ticket = await _context.Tickets.FindAsync(model.Id);
        if (ticket == null)
            throw new NotFoundException(nameof(Ticket), model.Id);

        if (!await _projectService.IsProjectMemberAsync(ticket.ProjectId, userId))
            throw new ForbiddenException("You do not have access to update this ticket.");

        ticket.Title = model.Title;
        ticket.Description = model.Description;
        ticket.Priority = model.Priority;
        ticket.Status = model.Status;
        ticket.AssignedToId = model.AssignedToId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteTicketAsync(int id, string userId)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            throw new NotFoundException(nameof(Ticket), id);

        if (!await _projectService.IsProjectMemberAsync(ticket.ProjectId, userId))
            throw new ForbiddenException("You do not have access to delete this ticket.");

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, TicketStatus newStatus, string userId)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            throw new NotFoundException(nameof(Ticket), id);

        if (!await _projectService.IsProjectMemberAsync(ticket.ProjectId, userId))
            throw new ForbiddenException("You do not have access to update this ticket.");

        ticket.Status = newStatus;
        await _context.SaveChangesAsync();
    }
}
