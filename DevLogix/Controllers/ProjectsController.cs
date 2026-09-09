namespace DevLogix.Controllers;

using DevLogix.Models;
using DevLogix.Services;
using DevLogix.ViewModels.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectsController(IProjectService projectService, UserManager<ApplicationUser> userManager)
    {
        _projectService = projectService;
        _userManager = userManager;
    }

    private string GetCurrentUserId() => _userManager.GetUserId(User) ?? string.Empty;
    private async Task<bool> IsAdmin() => User.IsInRole("Admin");

    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetAllProjectsAsync(GetCurrentUserId(), await IsAdmin());
        return View(projects);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,PM")]
    public IActionResult Create()
    {
        return View(new ProjectCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,PM")]
    public async Task<IActionResult> Create(ProjectCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        int projectId = await _projectService.CreateProjectAsync(model, GetCurrentUserId());
        TempData["SuccessMessage"] = "Project created successfully.";
        return RedirectToAction(nameof(Details), new { id = projectId });
    }

    public async Task<IActionResult> Details(int id)
    {
        var project = await _projectService.GetProjectDetailsAsync(id, GetCurrentUserId());
        return View(project);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,PM")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _projectService.GetProjectDetailsAsync(id, GetCurrentUserId());
        var model = new ProjectEditViewModel
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            IsActive = project.IsActive
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,PM")]
    public async Task<IActionResult> Edit(ProjectEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _projectService.UpdateProjectAsync(model, GetCurrentUserId());
        TempData["SuccessMessage"] = "Project updated successfully.";
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _projectService.GetProjectDetailsAsync(id, GetCurrentUserId());
        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _projectService.DeleteProjectAsync(id, GetCurrentUserId());
        TempData["SuccessMessage"] = "Project deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,PM")]
    public async Task<IActionResult> AddMember(int projectId, string userId)
    {
        await _projectService.AddMemberAsync(projectId, userId, GetCurrentUserId());
        TempData["SuccessMessage"] = "Member added successfully.";
        return RedirectToAction(nameof(Details), new { id = projectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,PM")]
    public async Task<IActionResult> RemoveMember(int projectId, string userId)
    {
        await _projectService.RemoveMemberAsync(projectId, userId, GetCurrentUserId());
        TempData["SuccessMessage"] = "Member removed successfully.";
        return RedirectToAction(nameof(Details), new { id = projectId });
    }
}
