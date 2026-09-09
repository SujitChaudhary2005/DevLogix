namespace DevLogix.Services;

using System.Threading.Tasks;

public interface ITimeLogService
{
    Task AddTimeLogAsync(int ticketId, int minutes, string? note, string userId);
}
