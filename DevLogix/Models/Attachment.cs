namespace DevLogix.Models;

public class Attachment
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public string UploadedById { get; set; } = string.Empty;
    public ApplicationUser UploadedBy { get; set; } = null!;

    public string FileName { get; set; } = string.Empty;
    public string StoredPath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

