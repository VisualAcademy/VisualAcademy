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

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        User user;

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            var username = credentials[0];
            var password = credentials[1];
            user = await _userService.Authenticate(username, password);
        }
        catch
        {
            return AuthenticateResult.Fail("Error Occurred. Authorization failed.");
        }

        if (user == null)
        {
            return AuthenticateResult.Fail("Invalid Credentials");
        }

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username)
            };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

//using System;
//using System.Text;

//// ����� �̸��� ��й�ȣ ����
//string username = "Administrator";
//string password = "Pa$$w0rd";

//// ũ�������� �ϳ��� ���ڿ��� ����
//string credentials = $"{username}:{password}";

//// ũ�������� ����Ʈ �迭�� ��ȯ
//byte[] bytes = Encoding.UTF8.GetBytes(credentials);

//// ����Ʈ �迭�� Base64�� ���ڵ�
//string encodedCredentials = Convert.ToBase64String(bytes);

//// ���ڵ��� ũ������ ���
//Console.WriteLine("Encoded Credentials: " + encodedCredentials);
