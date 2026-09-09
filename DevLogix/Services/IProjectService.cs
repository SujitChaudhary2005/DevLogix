namespace DevLogix.Services;

using DevLogix.ViewModels.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProjectService
{
    Task<IEnumerable<ProjectListViewModel>> GetAllProjectsAsync(string userId, bool isAdmin);
    Task<ProjectDetailsViewModel> GetProjectDetailsAsync(int id, string userId);
    Task<int> CreateProjectAsync(ProjectCreateViewModel model, string userId);
    Task UpdateProjectAsync(ProjectEditViewModel model, string userId);
    Task DeleteProjectAsync(int id, string userId);
    Task AddMemberAsync(int projectId, string userId, string currentUserId);
    Task RemoveMemberAsync(int projectId, string userId, string currentUserId);
    Task<bool> IsProjectMemberAsync(int projectId, string userId);
}
