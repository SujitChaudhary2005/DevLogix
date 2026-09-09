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
public class TicketsApiController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ICommentService _commentService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TicketsApiController(
        ITicketService ticketService,
        ICommentService commentService,
        UserManager<ApplicationUser> userManager)
    {
        _ticketService = ticketService;
        _commentService = commentService;
        _userManager = userManager;
    }

    private string GetCurrentUserId() => _userManager.GetUserId(User)!;

    /// <summary>
    /// GET /api/tickets?projectId=1&status=InProgress&priority=High&assignee=John&page=1&pageSize=25
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetTickets(
        int projectId,
        [FromQuery] TicketStatus? status = null,
        [FromQuery] Priority? priority = null,
        [FromQuery] string? assignee = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25)
    {
        var result = await _ticketService.GetTicketsAsync(
            projectId, GetCurrentUserId(), status, priority, assignee, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// GET /api/tickets/{id}
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(int id)
    {
        var ticket = await _ticketService.GetTicketDetailsAsync(id, GetCurrentUserId());
        return Ok(ticket);
    }

    /// <summary>
    /// PUT /api/tickets/{id}/status
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        await _ticketService.UpdateStatusAsync(id, request.NewStatus, GetCurrentUserId());
        return NoContent();
    }

    /// <summary>
    /// GET /api/tickets/{id}/comments
    /// </summary>
    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetComments(int id)
    {
        var ticket = await _ticketService.GetTicketDetailsAsync(id, GetCurrentUserId());
        return Ok(ticket.Comments);
    }

    /// <summary>
    /// POST /api/tickets/{id}/comments
    /// </summary>
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentRequest request)
    {
        await _commentService.AddCommentAsync(id, request.Content, GetCurrentUserId());
        return Created();
    }
}

public class UpdateStatusRequest
{
    public TicketStatus NewStatus { get; set; }
}

public class AddCommentRequest
{
    public string Content { get; set; } = string.Empty;
}
