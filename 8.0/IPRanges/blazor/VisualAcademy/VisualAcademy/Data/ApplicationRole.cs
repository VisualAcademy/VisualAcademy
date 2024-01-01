using Microsoft.AspNetCore.Identity;

namespace VisualAcademy.Data
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
