using Microsoft.AspNetCore.Mvc;
using VisualAcademy.Components;

var builder = WebApplication.CreateBuilder(args);

// Startup.ConfigureServices
// This method gets called by the runtime. Use this method to add services to the container.
// Add services to the container.
//builder.Services.AddMvc();
//builder.Services.AddControllers(); // Web API
builder.Services.AddControllersWithViews(); // MVC + Web API
builder.Services.AddRazorPages(); // Razor Pages
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Blazor Server

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Startup.Configure
// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
// app.UseDefaultFiles();
//app.UseStaticFiles(); // 정적인 HTML, CSS, JS, 이미지 파일 등을 제공하는 미들웨어
app.MapStaticAssets(); // .NET 9.0 
#endregion

app.UseRouting(); // 라우팅 미들웨어

app.UseAntiforgery(); // CSRF(Cross-Site Request Forgery) 공격 방지

//app.MapDefaultControllerRoute();
//app.MapControllers(); // Web API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Razor Pages

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Blazor Server

app.Run();

#region MVC
// ~/Mvc/Hello
public class MvcController : Controller
{
    public string Hello() => "Hello, MVC";
}
#endregion
