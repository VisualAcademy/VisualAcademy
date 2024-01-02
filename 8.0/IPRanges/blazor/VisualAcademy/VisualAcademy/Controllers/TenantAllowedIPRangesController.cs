using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualAcademy.Data;
using VisualAcademy.Models;

namespace VisualAcademy.Controllers
{
    [Authorize] // 로그인된 사용자만 접근 가능
    public class TenantAllowedIPRangesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TenantAllowedIPRangesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 현재 사용자의 TenantId를 가져오는 메서드
        private async Task<long?> GetCurrentTenantId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.TenantId; // null 반환 가능성을 명시
        }

        // Index: 현재 사용자의 TenantId에 해당하는 AllowedIPRanges 목록을 표시
        public async Task<IActionResult> Index()
        {
            var tenantId = await GetCurrentTenantId();
            if (tenantId == null)
            {
                return NotFound("Tenant ID not found for the current user.");
            }
            var allowedIPRanges = _context.AllowedIPRanges.Where(a => a.TenantId == tenantId);
            return View(await allowedIPRanges.ToListAsync());
        }

        // Details: 특정 AllowedIPRange의 세부 정보를 표시
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenantId = await GetCurrentTenantId();
            if (tenantId == null)
            {
                return NotFound("Tenant ID not found for the current user.");
            }

            var allowedIPRange = await _context.AllowedIPRanges
                .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);

            if (allowedIPRange == null)
            {
                return NotFound();
            }

            return View(allowedIPRange);
        }

        // Create: 새 AllowedIPRange를 생성
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartIPRange,EndIPRange,Description")] AllowedIPRange allowedIPRange)
        {
            var tenantId = await GetCurrentTenantId();
            if (tenantId == null)
            {
                return NotFound("Tenant ID not found for the current user.");
            }

            if (ModelState.IsValid)
            {
                allowedIPRange.TenantId = tenantId.Value;
                _context.Add(allowedIPRange);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allowedIPRange);
        }

        // Edit: 기존 AllowedIPRange를 수정
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenantId = await GetCurrentTenantId();
            if (tenantId == null)
            {
                return NotFound("Tenant ID not found for the current user.");
            }

            var allowedIPRange = await _context.AllowedIPRanges
                .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);

            if (allowedIPRange == null)
            {
                return NotFound();
            }

            return View(allowedIPRange);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartIPRange,EndIPRange,Description")] AllowedIPRange allowedIPRange)
        {
            if (id != allowedIPRange.Id)
            {
                return NotFound();
            }

            var tenantId = await GetCurrentTenantId();
            if (tenantId == null)
            {
                return NotFound("Tenant ID not found for the current user.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (allowedIPRange.TenantId != tenantId)
                    {
                        return NotFound();
                    }

                    _context.Update(allowedIPRange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllowedIPRangeExists(allowedIPRange.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(allowedIPRange);
        }

        // Delete: 특정 AllowedIPRange를 삭제
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenantId = await GetCurrentTenantId();
            if (tenantId == null)
            {
                return NotFound("Tenant ID not found for the current user.");
            }

            var allowedIPRange = await _context.AllowedIPRanges
                .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);

            if (allowedIPRange == null)
            {
                return NotFound();
            }

            return View(allowedIPRange);
        }

        // 실제로 삭제하는 POST 메서드
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tenantId = await GetCurrentTenantId();
            if (tenantId == null)
            {
                return NotFound("Tenant ID not found for the current user.");
            }

            var allowedIPRange = await _context.AllowedIPRanges
                .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);

            if (allowedIPRange != null)
            {
                _context.AllowedIPRanges.Remove(allowedIPRange);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AllowedIPRangeExists(int id)
        {
            var tenantId = _userManager.GetUserAsync(User).Result?.TenantId; // 동기적 방식으로 현재 사용자의 TenantId를 가져옴
            return _context.AllowedIPRanges.Any(e => e.Id == id && e.TenantId == tenantId);
        }
    }
}
