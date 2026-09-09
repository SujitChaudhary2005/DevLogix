namespace DevLogix.ViewModels.Projects;

using System.ComponentModel.DataAnnotations;

public class ProjectCreateViewModel
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
}
