namespace DevLogix.ViewModels.Wizard;

using System.ComponentModel.DataAnnotations;

public class WizardStep1ViewModel
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    [Display(Name = "Project Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
}
