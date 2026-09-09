namespace DevLogix.Services;

using DevLogix.Data;
using DevLogix.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class AttachmentService : IAttachmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public static readonly string[] AllowedAttachmentExtensions = { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".txt", ".zip", ".csv" };
    public static readonly int[] PageSizeOptions = { 10, 25, 50, 100 }; // Array demo for syllabus

    public AttachmentService(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<int> UploadAsync(int ticketId, IFormFile file, string userId)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        if (file.Length > 10 * 1024 * 1024)
            throw new ArgumentException("File exceeds 10MB limit");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedAttachmentExtensions.Contains(extension))
            throw new ArgumentException("File type not allowed");

        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null)
            throw new ArgumentException("Ticket not found");

        var uploadDir = Path.Combine(_env.ContentRootPath, "App_Data", "Uploads", ticketId.ToString());
        Directory.CreateDirectory(uploadDir);

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var storedPath = Path.Combine(uploadDir, uniqueFileName);

        using (var stream = new FileStream(storedPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            FileName = file.FileName,
            StoredPath = storedPath,
            ContentType = file.ContentType,
            FileSizeBytes = file.Length,
            TicketId = ticketId,
            UploadedById = userId
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return attachment.Id;
    }

    public async Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int attachmentId, string userId)
    {
        var attachment = await _context.Attachments
            .Include(a => a.Ticket)
            .ThenInclude(t => t.Project)
            .ThenInclude(p => p.Members)
            .FirstOrDefaultAsync(a => a.Id == attachmentId);

        if (attachment == null || !File.Exists(attachment.StoredPath))
            return null;

        // Security check could go here
        
        var stream = new FileStream(attachment.StoredPath, FileMode.Open, FileAccess.Read);
        return (stream, attachment.ContentType, attachment.FileName);
    }

    public async Task DeleteAsync(int attachmentId, string userId)
    {
        var attachment = await _context.Attachments.FindAsync(attachmentId);
        if (attachment == null) return;

        // Assuming basic security check passed elsewhere, but normally we'd check here too.
        
        if (File.Exists(attachment.StoredPath))
        {
            File.Delete(attachment.StoredPath);
        }

        _context.Attachments.Remove(attachment);
        await _context.SaveChangesAsync();
    }
}
