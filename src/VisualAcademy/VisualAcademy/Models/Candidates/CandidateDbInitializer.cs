namespace VisualAcademy.Models.Candidates;

public static class CandidateDbInitializer
{
    public static void Initialize(IApplicationBuilder applicationBuilder)
    {
        // https://docs.microsoft.com/ko-kr/aspnet/core/fundamentals/dependency-injection
        // ?view=aspnetcore-6.0#resolve-a-service-at-app-start-up
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var candidateDbContext = services.GetRequiredService<CandidateAppDbContext>();

            if (!candidateDbContext.Candidates.Any())
            {
                candidateDbContext.Candidates.Add(
                    new Candidate { FirstName = "꺽정", LastName = "임", IsEnrollment = false });

                candidateDbContext.SaveChanges();
            }
        }
    }
}
