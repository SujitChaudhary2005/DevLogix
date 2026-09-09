namespace DevLogix.Services;

using DevLogix.Data;
using DevLogix.Exceptions;
using DevLogix.Models;
using DevLogix.ViewModels.Projects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectListViewModel>> GetAllProjectsAsync(string userId, bool isAdmin)
    {
        var query = _context.Projects
            .Include(p => p.Members)
            .Include(p => p.Tickets)
            .AsQueryable();

        if (!isAdmin)
        {
            query = query.Where(p => p.Members.Any(m => m.UserId == userId));
        }

        var projects = await query.ToListAsync();

        return projects.Select(p => new ProjectListViewModel
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            IsActive = p.IsActive,
            MemberCount = p.Members.Count,
            TicketCount = p.Tickets.Count
        });
    }

    public async Task<ProjectDetailsViewModel> GetProjectDetailsAsync(int id, string userId)
    {
        var project = await _context.Projects
            .Include(p => p.Members)
                .ThenInclude(m => m.User)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.AssignedTo)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            throw new NotFoundException(nameof(Project), id);

        if (!await IsProjectMemberInternalAsync(project, userId))
            throw new ForbiddenException("You do not have access to this project.");

        var tickets = project.Tickets.OrderByDescending(t => t.CreatedAt).Take(10).ToList();

        return new ProjectDetailsViewModel
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            IsActive = project.IsActive,
            Members = project.Members.Select(m => new MemberViewModel
            {
                UserId = m.UserId,
                FullName = m.User.FullName,
                Email = m.User.Email,
                JoinedAt = m.JoinedAt
            }).ToList(),
            RecentTickets = tickets.Select(t => new TicketSummaryViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                Priority = t.Priority,
                AssignedToName = t.AssignedTo?.FullName,
                CreatedAt = t.CreatedAt
            }).ToList(),
            Metrics = new TicketMetrics
            {
                Total = project.Tickets.Count,
                Open = project.Tickets.Count(t => t.Status != TicketStatus.Resolved && t.Status != TicketStatus.Closed),
                Resolved = project.Tickets.Count(t => t.Status == TicketStatus.Resolved),
                Closed = project.Tickets.Count(t => t.Status == TicketStatus.Closed)
            }
        };
    }

    public async Task<int> CreateProjectAsync(ProjectCreateViewModel model, string userId)
    {
        var project = new Project
        {
            Title = model.Title,
            Description = model.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var member = new ProjectMember
        {
            ProjectId = project.Id,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        };

        _context.ProjectMembers.Add(member);
        await _context.SaveChangesAsync();

        return project.Id;
    }

    public async Task UpdateProjectAsync(ProjectEditViewModel model, string userId)
    {
        var project = await _context.Projects.FindAsync(model.Id);
        if (project == null)
            throw new NotFoundException(nameof(Project), model.Id);

        project.Title = model.Title;
        project.Description = model.Description;
        project.IsActive = model.IsActive;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(int id, string userId)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            throw new NotFoundException(nameof(Project), id);

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task AddMemberAsync(int projectId, string userId, string currentUserId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            throw new NotFoundException(nameof(Project), projectId);

        var existing = await _context.ProjectMembers
            .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);

        if (existing != null)
            return;

        var member = new ProjectMember
        {
            ProjectId = projectId,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        };

        _context.ProjectMembers.Add(member);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveMemberAsync(int projectId, string userId, string currentUserId)
    {
        var member = await _context.ProjectMembers
            .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);

        if (member == null)
            throw new NotFoundException(nameof(ProjectMember), $"{projectId}-{userId}");

        _context.ProjectMembers.Remove(member);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsProjectMemberAsync(int projectId, string userId)
    {
        var project = await _context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == projectId);
            
        if (project == null) return false;
        
        return await IsProjectMemberInternalAsync(project, userId);
    }
    
    private async Task<bool> IsProjectMemberInternalAsync(Project project, string userId)
    {
        // Admins check is typically done at the controller level or via user role checks
        // For pure service logic, if we need admin check we'd inject UserManager or similar,
        // but user roles are usually not directly checked here unless we pass isAdmin.
        // Assuming controller already checked admin or passes it down.
        // I will just check membership. The prompt says "Verifies project membership before all operations (non-admin)"
        // Since we don't have roles in project object, we'll assume the caller passes the correct context, or we check membership directly.
        var user = await _context.Users.FindAsync(userId);
        var userRoles = _context.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToList();
        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        bool isAdmin = adminRole != null && userRoles.Contains(adminRole.Id);

        return isAdmin || project.Members.Any(m => m.UserId == userId);
    }
}
