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
    // �� �������� �α��� ���� ������ �� �ֵ��� ����
    [AllowAnonymous]
    public class InitialPasswordModel : PageModel
    {
        // ����� �� �α��� ������ ���� ���� ����
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // �����ڸ� ���� ���� ����
        public InitialPasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ���ε��� �Է� �� ����
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            // �̸��� �ּ� �ʼ� �Է�
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            // ��ȣ �ʼ� �Է�
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            // ��ȣ Ȯ�� �ʵ�, ��ȣ�� ��ġ�ؾ� ��
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // �ʴ� �ڵ�
            public string Code { get; set; }

            // �߰����� �ʴ� �ڵ� (�ʿ��� ���)
            public string Code1 { get; set; }
        }

        // GET ��û ó��: �ʴ� �ڵ带 ����Ͽ� ������ �ε�
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

        // POST ��û ó��: ������� ��ȣ ����
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // ����ڰ� �������� ������ �ܺο� �巯���� ����
                return RedirectToPage("./SetPasswordConfirmation");
            }

            // ������� ��ȣ �缳�� �õ�
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            IdentityResult emailConfirmed = new();
            if (result.Succeeded)
            {
                // ��ȣ �缳�� ����, �̸��� Ȯ�� ���� ������Ʈ
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return RedirectToPage("./SetPasswordConfirmation");
            }

            // ���� ó��
            foreach (var error in result.Errors.Concat(emailConfirmed.Errors))
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
