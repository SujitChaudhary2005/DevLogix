namespace DevLogix.Services;

using DevLogix.Data;
using DevLogix.Exceptions;
using DevLogix.Models;
using System;
using System.Threading.Tasks;

public class TimeLogService : ITimeLogService
{
    private readonly ApplicationDbContext _context;
    private readonly IProjectService _projectService;

    public TimeLogService(ApplicationDbContext context, IProjectService projectService)
    {
        _context = context;
        _projectService = projectService;
    }

    public async Task AddTimeLogAsync(int ticketId, int minutes, string? note, string userId)
    {
        if (minutes <= 0)
            throw new BusinessRuleException("Minutes spent must be greater than zero.");

        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null)
            throw new NotFoundException(nameof(Ticket), ticketId);

        if (!await _projectService.IsProjectMemberAsync(ticket.ProjectId, userId))
            throw new ForbiddenException("You do not have access to log time on this ticket.");

        var timeLog = new TimeLog
        {
            TicketId = ticketId,
            MinutesSpent = minutes,
            Note = note ?? string.Empty,
            UserId = userId,
            LoggedAt = DateTime.UtcNow
        };

        _context.TimeLogs.Add(timeLog);
        await _context.SaveChangesAsync();
    }
}
