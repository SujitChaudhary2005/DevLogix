namespace DevLogix.Controllers;

using DevLogix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class TimeLogsController : Controller
{
    private readonly ITimeLogService _timeLogService;

    public TimeLogsController(ITimeLogService timeLogService)
    {
        _timeLogService = timeLogService;
    }

    private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int ticketId, int minutes, string? note)
    {
        await _timeLogService.AddTimeLogAsync(ticketId, minutes, note, GetCurrentUserId());
        TempData["SuccessMessage"] = "Time logged successfully.";
        return RedirectToAction("Details", "Tickets", new { id = ticketId });
    }
}
