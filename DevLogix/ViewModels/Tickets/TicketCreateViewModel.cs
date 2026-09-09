namespace DevLogix.ViewModels.Tickets;

using DevLogix.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class TicketCreateViewModel
{
    public int ProjectId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Priority Priority { get; set; }

    [Display(Name = "Assign To")]
    public string? AssignedToId { get; set; }

    public SelectList? ProjectMembers { get; set; }
}
