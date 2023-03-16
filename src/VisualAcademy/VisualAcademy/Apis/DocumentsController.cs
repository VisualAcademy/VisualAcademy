using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Apis;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    #region Version and Creator
    // api/documents/version
    [HttpGet("version")]
    public string GetVersion() => "1.0";

    // api/documents/creator
    [HttpGet("creator")]
    public JsonResult GetDocument()
        => new(new List<object>
        {
            new { Id = 1, Name = "VisualAcademy" },
            new { Id = 2, Name = "DevLec" }
        }); 
    #endregion

    // GET: api/<DocumentsController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<DocumentsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<DocumentsController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<DocumentsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<DocumentsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
