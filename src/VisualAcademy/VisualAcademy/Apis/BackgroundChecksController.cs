namespace VisualAcademy.Apis;

[Route("api/backgroundchecks")]
public class BackgroundChecksController : ControllerBase
{
    [HttpGet(template: "description")]
    public IActionResult GetDescription()
        => Ok(new { Description = "VisualAcademy Background Check" });
}
