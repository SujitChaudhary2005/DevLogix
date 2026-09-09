namespace DevLogix.ViewModels.Wizard;

using System.ComponentModel.DataAnnotations;

public class WizardStep2ViewModel
{
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Default Ticket Priority")]
    public string DefaultPriority { get; set; } = "Medium";
}
