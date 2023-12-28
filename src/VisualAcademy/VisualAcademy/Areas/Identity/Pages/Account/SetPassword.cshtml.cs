#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VisualAcademy.Data;

namespace VisualAcademy.Areas.Identity.Pages.Account
{
    // 이 페이지는 로그인 없이 접근할 수 있도록 설정
    [AllowAnonymous]
    public class InitialPasswordModel : PageModel
    {
        // 사용자 및 로그인 관리를 위한 서비스 주입
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // 생성자를 통해 서비스 주입
        public InitialPasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 바인딩할 입력 모델 정의
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            // 이메일 주소 필수 입력
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            // 암호 필수 입력
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            // 암호 확인 필드, 암호와 일치해야 함
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // 초대 코드
            public string Code { get; set; }

            // 추가적인 초대 코드 (필요한 경우)
            public string Code1 { get; set; }
        }

        // GET 요청 처리: 초대 코드를 사용하여 페이지 로드
        public async Task<IActionResult> OnGetAsync(string code = null, string code1 = null)
        {
            await _signInManager.SignOutAsync();
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                    Code1 = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code1)),
                };
                return Page();
            }
        }

        // POST 요청 처리: 사용자의 암호 설정
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // 사용자가 존재하지 않음을 외부에 드러내지 않음
                return RedirectToPage("./SetPasswordConfirmation");
            }

            // 사용자의 암호 재설정 시도
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            IdentityResult emailConfirmed = new();
            if (result.Succeeded)
            {
                // 암호 재설정 성공, 이메일 확인 상태 업데이트
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return RedirectToPage("./SetPasswordConfirmation");
            }

            // 에러 처리
            foreach (var error in result.Errors.Concat(emailConfirmed.Errors))
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
