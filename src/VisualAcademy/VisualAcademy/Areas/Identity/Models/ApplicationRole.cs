using Microsoft.AspNetCore.Identity;

namespace VisualAcademy.Areas.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
