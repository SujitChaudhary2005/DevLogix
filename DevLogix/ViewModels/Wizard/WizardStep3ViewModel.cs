namespace DevLogix.ViewModels.Wizard;

public class WizardStep3ViewModel
{
    // Review step — all data comes from session
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string DefaultPriority { get; set; } = string.Empty;
}
