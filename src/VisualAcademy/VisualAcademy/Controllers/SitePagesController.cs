using Azunt.Web.Data;
using Azunt.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Azunt.Web.Controllers;

public class SitePagesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly SitePageRouteSyncService _sync;

    public SitePagesController(
        ApplicationDbContext context,
        SitePageRouteSyncService sync)
    {
        _context = context;
        _sync = sync;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.SitePages
            .OrderBy(x => x.SortOrder)
            .ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Sync()
    {
        await _sync.SyncAsync();
        return RedirectToAction(nameof(Index));
    }
}