#nullable disable
using System.ComponentModel.DataAnnotations;

namespace VisualAcademy.Models.AccountViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
