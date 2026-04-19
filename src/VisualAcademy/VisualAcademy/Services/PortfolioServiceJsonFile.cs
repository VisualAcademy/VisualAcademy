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
        using var jsonFileReader = File.OpenText(JsonFileName);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var portfolios = JsonSerializer.Deserialize<Portfolio[]>(
            jsonFileReader.ReadToEnd(),
            options);

        return portfolios ?? Array.Empty<Portfolio>();
    }

    public void AddRating(int portfolioId, int rating)
    {
        var portfolios = GetPortfolios().ToList();

        var portfolio = portfolios.First(p => p.Id == portfolioId);

        if (portfolio.Ratings == null)
        {
            portfolio.Ratings = new int[] { rating };
        }
        else
        {
            var ratings = portfolio.Ratings.ToList();
            ratings.Add(rating);
            portfolio.Ratings = ratings.ToArray();
        }

        using var outputStream = File.OpenWrite(JsonFileName);
        outputStream.SetLength(0);

        JsonSerializer.Serialize(
            new Utf8JsonWriter(outputStream, new JsonWriterOptions
            {
                SkipValidation = true,
                Indented = true
            }),
            portfolios);
    }
}