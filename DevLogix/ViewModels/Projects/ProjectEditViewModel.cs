namespace DevLogix.ViewModels.Projects;

using System.ComponentModel.DataAnnotations;

public class ProjectEditViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
