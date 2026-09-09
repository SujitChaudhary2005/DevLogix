namespace DevLogix.Authorization;

using DevLogix.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class CanManageProjectHandler : AuthorizationHandler<CanManageProjectRequirement, int>
{
    private readonly ApplicationDbContext _context;

    public CanManageProjectHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanManageProjectRequirement requirement,
        int projectId)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return;

        // Admins can manage any project
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        // PM who is a member can manage
        if (context.User.IsInRole("PM"))
        {
            var isMember = await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
            if (isMember)
            {
                context.Succeed(requirement);
            }
        }
    }
}
