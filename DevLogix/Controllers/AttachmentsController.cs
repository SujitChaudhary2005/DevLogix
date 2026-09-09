namespace DevLogix.Controllers;

using DevLogix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class AttachmentsController : Controller
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentsController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(int ticketId, IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        try
        {
            await _attachmentService.UploadAsync(ticketId, file, userId);
            TempData["SuccessMessage"] = "Attachment uploaded successfully.";
        }
        catch (ArgumentException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction("Details", "Tickets", new { id = ticketId });
    }

    [HttpGet]
    public async Task<IActionResult> Download(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _attachmentService.DownloadAsync(id, userId);
        if (result == null) return NotFound();

        return File(result.Value.stream, result.Value.contentType, result.Value.fileName);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, int ticketId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        await _attachmentService.DeleteAsync(id, userId);
        TempData["SuccessMessage"] = "Attachment deleted.";
        
        return RedirectToAction("Details", "Tickets", new { id = ticketId });
    }
}
