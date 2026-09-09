namespace DevLogix.Services;

public interface IExportService
{
    Task<byte[]> ExportTicketsToCsvAsync(int? projectId = null);
}
