using RedPlus.Models;
using System.Text.Json;

namespace RedPlus.Services;

public class PortfolioServiceJsonFile(IWebHostEnvironment webHostEnvironment)
{
    /// <summary>
    /// wwwroot/Portfolios/portfolios.json 파일의 물리적인 경로 읽어오기 
    /// </summary>
    private string JsonFileName
    {
        get
        {
            return Path.Combine(webHostEnvironment.WebRootPath, "Portfolios", "portfolios.json");
        }
    }

    public IEnumerable<Portfolio> GetPortfolios()
    {
        // Using 선언: https://docs.microsoft.com/ko-kr/dotnet/csharp/whats-new/csharp-8#using-declarations
        using var jsonFileReader = File.OpenText(JsonFileName);
        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var portfolios = JsonSerializer.Deserialize<Portfolio[]>(jsonFileReader.ReadToEnd(), options);
        return portfolios;
    }

    public void AddRating(int portfolioId, int rating)
    {
        var portfolios = GetPortfolios();

        if (portfolios.First(p => p.Id == portfolioId).Ratings == null)
        {
            portfolios.First(p => p.Id == portfolioId).Ratings = new int[] { rating };
        }
        else
        {
            var ratings = portfolios.First(p => p.Id == portfolioId).Ratings.ToList();
            ratings.Add(rating);
            portfolios.First(p => p.Id == portfolioId).Ratings = ratings.ToArray();
        }

        using var outputStream = File.OpenWrite(JsonFileName);
        JsonSerializer.Serialize<IEnumerable<Portfolio>>(
            new Utf8JsonWriter(outputStream, new JsonWriterOptions
            {
                SkipValidation = true,
                Indented = true
            }), portfolios);
    }
}
