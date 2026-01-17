using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
}

public class User
{
    public string? Id { get; internal set; }
    public string? Username { get; internal set; }
}

public class UserService : IUserService
{
    public Task<User> Authenticate(string username, string password)
    {
        if (username != "Administrator" || password != "Pa$$w0rd")
        {
            return Task.FromResult<User>(null!);
        }

        var user = new User
        {
            Username = username,
            Id = Guid.NewGuid().ToString("N")
        };

        return Task.FromResult(user);
    }
}

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserService userService)
        : base(options, logger, encoder, clock)
    {
        _userService = userService;
    }

    /// <summary>
    /// HTTP Basic 인증 헤더를 읽어 사용자를 인증하고,
    /// 성공 시 ClaimsPrincipal 기반의 인증 티켓을 발급하는 핸들러.
    /// </summary>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        User user;

        try
        {
            // 1) Authorization 헤더 읽기
            var authHeaderValue = Request.Headers["Authorization"].FirstOrDefault();

            // 헤더가 없으면 즉시 실패 처리
            if (string.IsNullOrEmpty(authHeaderValue))
            {
                return AuthenticateResult.Fail("Missing Authorization header.");
            }

            // 2) Authorization 헤더 파싱 (Basic Base64 형식 가정)
            var authHeader = AuthenticationHeaderValue.Parse(authHeaderValue);

            // 3) Base64 디코딩
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);

            // 4) "username:password" 형식으로 분리
            var credentials = Encoding.UTF8
                                       .GetString(credentialBytes)
                                       .Split(new[] { ':' }, 2);

            var username = credentials[0];
            var password = credentials[1];

            // 5) 실제 사용자 인증 서비스 호출
            user = await _userService.Authenticate(username, password);
        }
        catch
        {
            // 파싱 오류, Base64 오류, 서비스 오류 등 모든 예외 처리
            return AuthenticateResult.Fail("Error Occurred. Authorization failed.");
        }

        // 6) 인증 실패 시 처리
        if (user == null)
        {
            return AuthenticateResult.Fail("Invalid Credentials");
        }

        // user.Id / user.Username가 null일 수 있으므로 안전하게 처리
        if (string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.Username))
        {
            return AuthenticateResult.Fail("Authenticated user has invalid identity data.");
        }

        // 7) 인증 성공 → 클레임 생성 (이제 경고 없음)
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.Username)
    };

        // 8) ClaimsIdentity 및 Principal 생성
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);

        // 9) 인증 티켓 발급
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        // 10) 최종 성공 반환
        return AuthenticateResult.Success(ticket);
    }
}

//using System;
//using System.Text;

//// 사용자 이름과 비밀번호 설정
//string username = "Administrator";
//string password = "Pa$$w0rd";

//// 크리덴셜을 하나의 문자열로 결합
//string credentials = $"{username}:{password}";

//// 크리덴셜을 바이트 배열로 변환
//byte[] bytes = Encoding.UTF8.GetBytes(credentials);

//// 바이트 배열을 Base64로 인코딩
//string encodedCredentials = Convert.ToBase64String(bytes);

//// 인코딩된 크리덴셜 출력
//Console.WriteLine("Encoded Credentials: " + encodedCredentials);
