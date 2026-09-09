namespace DevLogix.Controllers;

using DevLogix.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Health check endpoint for uptime monitoring.
/// </summary>
public class HealthController : Controller
{
    private readonly ApplicationDbContext _context;

    public HealthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("/health")]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Verify database connectivity
            await _context.Database.CanConnectAsync();

            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                database = "connected",
                version = "1.0.0"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new
            {
                status = "unhealthy",
                timestamp = DateTime.UtcNow,
                database = "disconnected",
                error = ex.Message
            });
        }
    }
}
