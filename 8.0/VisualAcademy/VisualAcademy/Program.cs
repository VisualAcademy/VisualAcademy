using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

var app = builder.Build();

#region HTTP
// C# 11.0 버전의 원시 문자열 리터럴(Raw string literals)
string htmlTag = """
<html lang="ko">
<body>
    <h1>Hello ASP.NET Core 8.0</h1>
</body>
</html>
""";

app.MapGet("/html-content-rendering", () => Results.Content(htmlTag, "text/html"));
#endregion

#region HTML
app.UseDefaultFiles();
app.UseStaticFiles(); 
#endregion

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();

#region MVC
// /Mvc/Hello
public class MvcController : Controller
{
    public string Hello() => "Hello, MVC";
}
#endregion
