namespace DevLogix.Services;

using DevLogix.Models;
using DevLogix.ViewModels.Tickets;
using System.Threading.Tasks;

public interface ITicketService
{
    Task<TicketListViewModel> GetTicketsAsync(int projectId, string userId, TicketStatus? status, Priority? priority, string? assignee, int page, int pageSize);
    Task<TicketDetailsViewModel> GetTicketDetailsAsync(int id, string userId);
    Task<int> CreateTicketAsync(TicketCreateViewModel model, string userId);
    Task UpdateTicketAsync(TicketEditViewModel model, string userId);
    Task DeleteTicketAsync(int id, string userId);
    Task UpdateStatusAsync(int id, TicketStatus newStatus, string userId);
}
