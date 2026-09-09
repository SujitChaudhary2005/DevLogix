namespace DevLogix.Controllers;

using DevLogix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin,PM")]
public class ExportController : Controller
{
    private readonly IExportService _exportService;

    public ExportController(IExportService exportService)
    {
        _exportService = exportService;
    }

    [HttpGet]
    public async Task<IActionResult> ExportTickets(int? projectId)
    {
        var csvBytes = await _exportService.ExportTicketsToCsvAsync(projectId);
        var fileName = projectId.HasValue ? $"tickets-project-{projectId}.csv" : "tickets-export.csv";
        return File(csvBytes, "text/csv", fileName);
    }
}
