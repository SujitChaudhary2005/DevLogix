namespace DevLogix.Controllers;

using DevLogix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class CommentsController : Controller
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int ticketId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            TempData["ErrorMessage"] = "Comment cannot be empty.";
            return RedirectToAction("Details", "Tickets", new { id = ticketId });
        }

        await _commentService.AddCommentAsync(ticketId, content, GetCurrentUserId());
        TempData["SuccessMessage"] = "Comment added successfully.";
        return RedirectToAction("Details", "Tickets", new { id = ticketId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int ticketId)
    {
        await _commentService.DeleteCommentAsync(id, GetCurrentUserId());
        TempData["SuccessMessage"] = "Comment deleted successfully.";
        return RedirectToAction("Details", "Tickets", new { id = ticketId });
    }
}
