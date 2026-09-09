namespace DevLogix.Controllers;

using DevLogix.Models;
using DevLogix.Services;
using DevLogix.ViewModels.Wizard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ProjectWizardController : Controller
{
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectWizardController(IProjectService projectService, UserManager<ApplicationUser> userManager)
    {
        _projectService = projectService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Step1()
    {
        var model = new WizardStep1ViewModel
        {
            Title = HttpContext.Session.GetString("Wizard_Title") ?? string.Empty,
            Description = HttpContext.Session.GetString("Wizard_Description") ?? string.Empty
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Step1(WizardStep1ViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        HttpContext.Session.SetString("Wizard_Title", model.Title);
        HttpContext.Session.SetString("Wizard_Description", model.Description ?? string.Empty);

        return RedirectToAction(nameof(Step2));
    }

    [HttpGet]
    public IActionResult Step2()
    {
        var model = new WizardStep2ViewModel
        {
            IsActive = HttpContext.Session.GetString("Wizard_IsActive") != "False",
            DefaultPriority = HttpContext.Session.GetString("Wizard_DefaultPriority") ?? "Medium"
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Step2(WizardStep2ViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        HttpContext.Session.SetString("Wizard_IsActive", model.IsActive.ToString());
        HttpContext.Session.SetString("Wizard_DefaultPriority", model.DefaultPriority);

        return RedirectToAction(nameof(Step3));
    }

    [HttpGet]
    public IActionResult Step3()
    {
        var model = new WizardStep3ViewModel
        {
            Title = HttpContext.Session.GetString("Wizard_Title") ?? string.Empty,
            Description = HttpContext.Session.GetString("Wizard_Description") ?? string.Empty,
            IsActive = HttpContext.Session.GetString("Wizard_IsActive") != "False",
            DefaultPriority = HttpContext.Session.GetString("Wizard_DefaultPriority") ?? "Medium"
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Complete()
    {
        var title = HttpContext.Session.GetString("Wizard_Title");
        var description = HttpContext.Session.GetString("Wizard_Description");
        var isActiveStr = HttpContext.Session.GetString("Wizard_IsActive");
        
        if (string.IsNullOrEmpty(title))
            return RedirectToAction(nameof(Step1));

        var project = new Project
        {
            Title = title,
            Description = description ?? string.Empty,
            IsActive = isActiveStr != "False"
        };

        var userId = _userManager.GetUserId(User);
        if (userId != null)
        {
            project.Members.Add(new ProjectMember { UserId = userId });
        }

        await _projectService.CreateProjectAsync(project);
        
        ClearSession();
        
        TempData["SuccessMessage"] = "Project created successfully!";
        return RedirectToAction("Details", "Projects", new { id = project.Id });
    }

    [HttpGet]
    public IActionResult Cancel()
    {
        ClearSession();
        return RedirectToAction("Index", "Projects");
    }

    private void ClearSession()
    {
        HttpContext.Session.Remove("Wizard_Title");
        HttpContext.Session.Remove("Wizard_Description");
        HttpContext.Session.Remove("Wizard_IsActive");
        HttpContext.Session.Remove("Wizard_DefaultPriority");
    }
}
