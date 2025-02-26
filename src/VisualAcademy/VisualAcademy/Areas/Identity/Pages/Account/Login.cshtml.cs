// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using All.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace VisualAcademy.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger, ApplicationDbContext context, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // 구성에서 IP 수집 및 제한 설정을 읽어옵니다.
            bool enableIPRestriction = _configuration.GetValue<bool>("IPRestriction:EnableIPRestriction");
            bool collectLoginIP = _configuration.GetValue<bool>("IPRestriction:CollectLoginIP");

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // 사용자 정보 검색
                var user = await _userManager.FindByEmailAsync(Input.Email);
                var TenantId = user.TenantId;

                // 현재 IP 주소 검색
                string currentIP = HttpContext.Connection.RemoteIpAddress.ToString();
                // 로컬호스트 IPv6 주소인 '::1'을 '127.0.0.1'로 변환
                if (currentIP == "::1")
                {
                    currentIP = "127.0.0.1";
                }

                // IP 수집이 활성화된 경우, 현재 IP를 수집합니다.
                if (collectLoginIP)
                {
                    try
                    {
                        var ipParts = currentIP.Split('.');
                        if (ipParts.Length == 4) // IPv4 주소인지 확인
                        {
                            // 마지막 옥텟을 1로 설정하여 StartIPRange 계산
                            ipParts[3] = "1";
                            string startIPRange = string.Join(".", ipParts);

                            // 마지막 옥텟을 255로 설정하여 EndIPRange 계산
                            ipParts[3] = "255";
                            string endIPRange = string.Join(".", ipParts);

                            // 사용자 이메일에서 도메인 부분만 추출
                            string emailDomain = Input.Email.Substring(Input.Email.IndexOf('@') + 1);

                            // 동일한 StartIPRange와 EndIPRange를 가진 엔트리가 이미 있는지 확인
                            var existingIPRange = await _context.AllowedIPRanges
                                .FirstOrDefaultAsync(ip => ip.StartIPRange == startIPRange && ip.EndIPRange == endIPRange && ip.TenantId == user.TenantId);

                            // 동일한 범위가 존재하지 않는 경우에만 새 범위 추가
                            if (existingIPRange == null)
                            {
                                var newIPRange = new AllowedIPRange
                                {
                                    StartIPRange = startIPRange,
                                    EndIPRange = endIPRange,
                                    Description = emailDomain, // 사용자 이메일 도메인으로 설명 설정
                                    CreateDate = DateTime.Now,
                                    TenantId = user.TenantId // 현재 로그인한 사용자의 TenantId 사용
                                };
                                _context.AllowedIPRanges.Add(newIPRange);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // 예외 로깅 또는 사용자에게 친절한 에러 메시지 표시
                        // 예: _logger.LogError("An error occurred: {0}", ex.ToString());
                        // 사용자에게는 특정 에러 메시지를 반환하거나, 에러 페이지로 리디렉션할 수 있습니다.
                    }
                }

                // IP 제한이 활성화된 경우, 현재 IP 주소를 검사합니다.
                if (enableIPRestriction)
                {
                    // IP 주소 허용 검사
                    bool isAllowed = await CheckIPAllowed(TenantId, currentIP);

                    if (!isAllowed)
                    {
                        // 허용되지 않은 IP 주소인 경우, RestrictedAccess 뷰로 리디렉션
                        return RedirectToPage("/RestrictedAccess");
                    }
                }

                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    // IP 허용 검사 메서드 구현
    private async Task<bool> CheckIPAllowed(long TenantId, string currentIP)
    {
        var ipRangeList = await _context.AllowedIPRanges
                                        .Where(r => r.TenantId == TenantId)
                                        .ToListAsync();

        // 특정 테넌트ID에 해당하는 등록된 허용IP주소가 없으면 제한을 걸지 않고 허용
        if (!ipRangeList.Any())
        {
            return true; // 등록된 허용된 IP 주소가 없으면 모든 접속을 허용
        }

        foreach (var range in ipRangeList)
        {
            if (IsIPInRange(currentIP, range.StartIPRange, range.EndIPRange))
            {
                return true; // 현재 IP가 허용된 범위 내에 있으면 true 반환
            }
        }

        return false; // 허용된 범위에 없으면 false 반환
    }

    // IP 범위 확인 메서드
    private bool IsIPInRange(string currentIP, string startIP, string endIP)
    {
        var addr = IPAddress.Parse(currentIP);
        var lowerBound = IPAddress.Parse(startIP);
        var upperBound = IPAddress.Parse(endIP);

        byte[] addrBytes = addr.GetAddressBytes();
        byte[] lowerBytes = lowerBound.GetAddressBytes();
        byte[] upperBytes = upperBound.GetAddressBytes();

        bool lowerBoundCheck = true;
        bool upperBoundCheck = true;

        for (int i = 0; i < addrBytes.Length && (lowerBoundCheck || upperBoundCheck); i++)
        {
            if (lowerBoundCheck)
            {
                if (addrBytes[i] < lowerBytes[i])
                {
                    return false;
                }
                else if (addrBytes[i] > lowerBytes[i])
                {
                    lowerBoundCheck = false;
                }
            }

            if (upperBoundCheck)
            {
                if (addrBytes[i] > upperBytes[i])
                {
                    return false;
                }
                else if (addrBytes[i] < upperBytes[i])
                {
                    upperBoundCheck = false;
                }
            }
        }
        return true;
    }
}
