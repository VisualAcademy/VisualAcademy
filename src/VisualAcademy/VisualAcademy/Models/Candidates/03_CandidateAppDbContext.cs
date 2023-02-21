using Microsoft.EntityFrameworkCore;
using VisualAcademy.Models.Candidates.CandidatesIncomes;
using VisualAcademy.Models.Candidates.CandidatesNames;

namespace VisualAcademy.Models.Candidates;

public class CandidateAppDbContext : DbContext
{
    public CandidateAppDbContext() : base()
    {
        // Empty
        // 만약, Repository 클래스에 생성자 매개 변수 주입 방식 사용시 이 생성자 제거 
    }

    public CandidateAppDbContext(DbContextOptions<CandidateAppDbContext> options)
        : base(options)
    {
        // Empty
    }

    // DbSet of T 형태의 컬렉션 속성을 사용하여 모델(도메인)에 해당하는 테이블 생성
    public DbSet<Candidate> Candidates { get; set; } = null!;

    public DbSet<CandidateName> CandidatesNames { get; set; } = null!;

    public DbSet<CandidateIncome> CandidatesIncomes { get; set; } = null!;
}
