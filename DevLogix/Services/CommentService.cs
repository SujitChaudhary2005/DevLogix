namespace DevLogix.Services;

using DevLogix.Data;
using DevLogix.Exceptions;
using DevLogix.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CommentService : ICommentService
{
    private readonly ApplicationDbContext _context;
    private readonly IProjectService _projectService;

    public CommentService(ApplicationDbContext context, IProjectService projectService)
    {
        _context = context;
        _projectService = projectService;
    }

    public async Task AddCommentAsync(int ticketId, string content, string userId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null)
            throw new NotFoundException(nameof(Ticket), ticketId);

        if (!await _projectService.IsProjectMemberAsync(ticket.ProjectId, userId))
            throw new ForbiddenException("You do not have access to comment on this ticket.");

        var comment = new Comment
        {
            TicketId = ticketId,
            Content = content,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId, string userId)
    {
        var comment = await _context.Comments
            .Include(c => c.Ticket)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment == null)
            throw new NotFoundException(nameof(Comment), commentId);

        // Check if user is admin or the comment author
        var userRoles = _context.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToList();
        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        bool isAdmin = adminRole != null && userRoles.Contains(adminRole.Id);

        if (!isAdmin && comment.UserId != userId)
            throw new ForbiddenException("You do not have permission to delete this comment.");

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
