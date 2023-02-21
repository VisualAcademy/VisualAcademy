using System.ComponentModel.DataAnnotations;

namespace VisualAcademy.Models.Candidates.CandidatesIncomes;

public class CandidateIncome
{
    public int Id { get; set; }

    [Required(ErrorMessage = "*")]
    [StringLength(50)]
    public string? Source { get; set; }

    public decimal? Amount { get; set; }

    public string? UserId { get; set; } = null;
}
