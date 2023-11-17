var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// C# 11.0 버전의 원시 문자열 리터럴(Raw string literals)
string htmlTag = """
<html lang="ko">
<body>
    <h1>Hello ASP.NET Core 8.0</h1>
</body>
</html>
""";

app.MapGet("/html-content-rendering", () => Results.Content(htmlTag, "text/html"));

app.Run();
