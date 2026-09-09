namespace DevLogix.Services;

using System.Threading.Tasks;

public interface ICommentService
{
    Task AddCommentAsync(int ticketId, string content, string userId);
    Task DeleteCommentAsync(int commentId, string userId);
}
