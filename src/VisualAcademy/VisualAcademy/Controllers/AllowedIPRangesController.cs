using Microsoft.EntityFrameworkCore;

namespace VisualAcademy.Controllers;

[Authorize(Roles = "Administrators")]
public class AllowedIPRangesController(ApplicationDbContext context) : Controller
{
    // GET: AllowedIPRanges
    public async Task<IActionResult> Index()
    {
        return View(await context.AllowedIPRanges.ToListAsync());
    }

    // GET: AllowedIPRanges/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var allowedIPRange = await context.AllowedIPRanges
            .FirstOrDefaultAsync(m => m.Id == id);
        if (allowedIPRange == null)
        {
            return NotFound();
        }

        return View(allowedIPRange);
    }

    // GET: AllowedIPRanges/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AllowedIPRanges/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,StartIPRange,EndIPRange,Description,CreateDate,TenantId")] AllowedIPRange allowedIPRange)
    {
        if (ModelState.IsValid)
        {
            context.Add(allowedIPRange);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(allowedIPRange);
    }

    // GET: AllowedIPRanges/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var allowedIPRange = await context.AllowedIPRanges.FindAsync(id);
        if (allowedIPRange == null)
        {
            return NotFound();
        }
        return View(allowedIPRange);
    }

    // POST: AllowedIPRanges/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StartIPRange,EndIPRange,Description,CreateDate,TenantId")] AllowedIPRange allowedIPRange)
    {
        if (id != allowedIPRange.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(allowedIPRange);
                await context.SaveChangesAsync();
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

    // GET: AllowedIPRanges/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var allowedIPRange = await context.AllowedIPRanges
            .FirstOrDefaultAsync(m => m.Id == id);
        if (allowedIPRange == null)
        {
            return NotFound();
        }

        return View(allowedIPRange);
    }

    // POST: AllowedIPRanges/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var allowedIPRange = await context.AllowedIPRanges.FindAsync(id);
        if (allowedIPRange != null)
        {
            context.AllowedIPRanges.Remove(allowedIPRange);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AllowedIPRangeExists(int id)
    {
        return context.AllowedIPRanges.Any(e => e.Id == id);
    }
}
