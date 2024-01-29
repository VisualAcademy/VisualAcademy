using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Apis;

[ApiController]
[Route("api/[controller]")]
public class HelloWebApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    { 
        return Ok("Hello Web API");
    }

    // JSON 형식으로 데이터를 반환하는 추가적인 액션 메서드
    [HttpGet("GetJson")]
    public IActionResult GetJson()
    {
        var data = new Dictionary<string, string>
        {
            { "Message", "Hello World from Web API!" },
            { "Author", "VisualAcademy" }
        };

        return new JsonResult(data);
    }
}
