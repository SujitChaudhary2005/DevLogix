namespace DevLogix.Services;

using Microsoft.AspNetCore.Http;

public interface IAttachmentService
{
    Task<int> UploadAsync(int ticketId, IFormFile file, string userId);
    Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int attachmentId, string userId);
    Task DeleteAsync(int attachmentId, string userId);
}
