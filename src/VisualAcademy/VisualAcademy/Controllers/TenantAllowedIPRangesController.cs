﻿using Azunt.Entities;

namespace VisualAcademy.Controllers;

[Authorize] // 로그인된 사용자만 접근 가능
public class TenantAllowedIpRangesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    // 현재 사용자의 TenantId를 가져오는 메서드
    private async Task<long?> GetCurrentTenantId()
    {
        var user = await userManager.GetUserAsync(User);
        return user?.TenantId; // null 반환 가능성을 명시
    }

    // Index: 현재 사용자의 TenantId에 해당하는 AllowedIpRanges 목록을 표시
    public async Task<IActionResult> Index()
    {
        var tenantId = await GetCurrentTenantId();
        if (tenantId == null)
        {
            return NotFound("Tenant ID not found for the current user.");
        }
        var allowedIPRanges = context.AllowedIpRanges.Where(a => a.TenantId == tenantId);
        return View(await allowedIPRanges.ToListAsync());
    }

    // Details: 특정 AllowedIpRange의 세부 정보를 표시
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

        var allowedIPRange = await context.AllowedIpRanges
            .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);

        if (allowedIPRange == null)
        {
            return NotFound();
        }

        return View(allowedIPRange);
    }

    // Create: 새 AllowedIpRange를 생성
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StartIpRange,EndIpRange,Description")] AllowedIpRange allowedIPRange)
    {
        var tenantId = await GetCurrentTenantId();
        if (tenantId == null)
        {
            return NotFound("Tenant ID not found for the current user.");
        }

        if (ModelState.IsValid)
        {
            allowedIPRange.TenantId = tenantId.Value;
            context.Add(allowedIPRange);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(allowedIPRange);
    }

    // Edit: 기존 AllowedIpRange를 수정
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

        var allowedIPRange = await context.AllowedIpRanges
            .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);

        if (allowedIPRange == null)
        {
            return NotFound();
        }

        return View(allowedIPRange);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StartIpRange,EndIpRange,Description")] AllowedIpRange allowedIPRange)
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

                context.Update(allowedIPRange);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllowedIpRangeExists(allowedIPRange.Id))
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

    // Delete: 특정 AllowedIpRange를 삭제
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

        var allowedIPRange = await context.AllowedIpRanges
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

        var allowedIPRange = await context.AllowedIpRanges
            .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);

        if (allowedIPRange != null)
        {
            context.AllowedIpRanges.Remove(allowedIPRange);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool AllowedIpRangeExists(int id)
    {
        var tenantId = userManager.GetUserAsync(User).Result?.TenantId; // 동기적 방식으로 현재 사용자의 TenantId를 가져옴
        return context.AllowedIpRanges.Any(e => e.Id == id && e.TenantId == tenantId);
    }
}
