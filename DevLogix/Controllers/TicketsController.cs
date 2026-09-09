namespace DevLogix.Controllers;

using DevLogix.Models;
using DevLogix.Services;
using DevLogix.ViewModels.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

[Authorize]
public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TicketsController(ITicketService ticketService, UserManager<ApplicationUser> userManager)
    {
        _ticketService = ticketService;
        _userManager = userManager;
    }

    private string GetCurrentUserId() => _userManager.GetUserId(User) ?? string.Empty;

    public async Task<IActionResult> Index(int projectId, TicketStatus? status, Priority? priority, string? assignee, int page = 1)
    {
        var model = await _ticketService.GetTicketsAsync(projectId, GetCurrentUserId(), status, priority, assignee, page, 10);
        return View(model);
    }

    [HttpGet]
    public IActionResult Create(int projectId)
    {
        var model = new TicketCreateViewModel { ProjectId = projectId };
        // Ideally populate ProjectMembers dropdown here
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TicketCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        int ticketId = await _ticketService.CreateTicketAsync(model, GetCurrentUserId());
        TempData["SuccessMessage"] = "Ticket created successfully.";
        return RedirectToAction(nameof(Details), new { id = ticketId });
    }

    public async Task<IActionResult> Details(int id)
    {
        var ticket = await _ticketService.GetTicketDetailsAsync(id, GetCurrentUserId());
        return View(ticket);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var ticket = await _ticketService.GetTicketDetailsAsync(id, GetCurrentUserId());
        var model = new TicketEditViewModel
        {
            Id = ticket.Id,
            ProjectId = ticket.ProjectId,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = ticket.Priority,
            Status = ticket.Status,
            AssignedToId = ticket.AssignedToId
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TicketEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _ticketService.UpdateTicketAsync(model, GetCurrentUserId());
        TempData["SuccessMessage"] = "Ticket updated successfully.";
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var ticket = await _ticketService.GetTicketDetailsAsync(id, GetCurrentUserId());
        return View(ticket);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ticket = await _ticketService.GetTicketDetailsAsync(id, GetCurrentUserId());
        await _ticketService.DeleteTicketAsync(id, GetCurrentUserId());
        TempData["SuccessMessage"] = "Ticket deleted successfully.";
        return RedirectToAction(nameof(Index), new { projectId = ticket.ProjectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, TicketStatus status)
    {
        await _ticketService.UpdateStatusAsync(id, status, GetCurrentUserId());
        TempData["SuccessMessage"] = "Ticket status updated.";
        return RedirectToAction(nameof(Details), new { id = id });
    }
}
