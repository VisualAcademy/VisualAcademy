#nullable disable
namespace VisualAcademy.Apis;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
}

[Authorize(Roles = "Administrators")]
[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly List<City> _cities;

    public CitiesController()
    {
        _cities = new List<City>
        {
            new City { Id = 1, Name = "Seoul" },
            new City { Id = 2, Name = "New York" },
            new City { Id = 3, Name = "London" },
            new City { Id = 4, Name = "Paris" },
            new City { Id = 5, Name = "Tokyo" },
        };
    }

    // GET api/cities
    [HttpGet]
    public ActionResult<IEnumerable<City>> GetCities() => _cities;

    // GET api/cities/5
    [HttpGet("{id}")]
    public ActionResult<City> GetCity(int id)
    {
        var city = _cities.FirstOrDefault(c => c.Id == id);

        if (city == null)
        {
            return NotFound();
        }

        return city;
    }

    // POST api/cities
    [HttpPost]
    public ActionResult<City> CreateCity([FromBody] City city)
    {
        city.Id = _cities.Max(c => c.Id) + 1;
        _cities.Add(city);
        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    }

    // PUT api/cities/5
    [HttpPut("{id}")]
    public ActionResult UpdateCity(int id, [FromBody] City city)
    {
        var existingCity = _cities.FirstOrDefault(c => c.Id == id);

        if (existingCity == null)
        {
            return NotFound();
        }

        existingCity.Name = city.Name;

        return NoContent();
    }

    // DELETE api/cities/5
    [HttpDelete("{id}")]
    public ActionResult DeleteCity(int id)
    {
        var city = _cities.FirstOrDefault(c => c.Id == id);

        if (city == null)
        {
            return NotFound();
        }

        _cities.Remove(city);

        return NoContent();
    }
}
