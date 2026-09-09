namespace DevLogix.Controllers.Api;

using DevLogix.Models;
using DevLogix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ProjectsApiController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectsApiController(
        IProjectService projectService,
        UserManager<ApplicationUser> userManager)
    {
        _projectService = projectService;
        _userManager = userManager;
    }

    private string GetCurrentUserId() => _userManager.GetUserId(User)!;
    private bool IsAdmin() => User.IsInRole("Admin");

    /// <summary>
    /// GET /api/projects — list projects scoped to caller's membership (unless Admin)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync(GetCurrentUserId(), IsAdmin());
        return Ok(projects);
    }

    /// <summary>
    /// POST /api/projects — create a new project (PM/Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,PM")]
    public async Task<IActionResult> CreateProject([FromBody] ViewModels.Projects.ProjectCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var projectId = await _projectService.CreateProjectAsync(model, GetCurrentUserId());
        return CreatedAtAction(nameof(GetProjects), new { id = projectId }, new { id = projectId });
    }
}
