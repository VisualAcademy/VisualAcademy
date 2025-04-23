using Azunt.Entities;

namespace VisualAcademy.Controllers;

[Authorize(Roles = "Administrators")]
public class AllowedIpRangesController(ApplicationDbContext context) : Controller
{
    // GET: AllowedIpRanges
    public async Task<IActionResult> Index() => View(await context.AllowedIpRanges.ToListAsync());

    // GET: AllowedIpRanges/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var allowedIPRange = await context.AllowedIpRanges
            .FirstOrDefaultAsync(m => m.Id == id);
        if (allowedIPRange == null)
        {
            return NotFound();
        }

        return View(allowedIPRange);
    }

    // GET: AllowedIpRanges/Create
    public IActionResult Create() => View();

    // POST: AllowedIpRanges/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,StartIpRange,EndIpRange,Description,CreateDate,TenantId")] AllowedIpRange allowedIPRange)
    {
        if (ModelState.IsValid)
        {
            context.Add(allowedIPRange);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(allowedIPRange);
    }

    // GET: AllowedIpRanges/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var allowedIPRange = await context.AllowedIpRanges.FindAsync(id);
        if (allowedIPRange == null)
        {
            return NotFound();
        }
        return View(allowedIPRange);
    }

    // POST: AllowedIpRanges/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StartIpRange,EndIpRange,Description,CreateDate,TenantId")] AllowedIpRange allowedIPRange)
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

    // GET: AllowedIpRanges/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var allowedIPRange = await context.AllowedIpRanges
            .FirstOrDefaultAsync(m => m.Id == id);
        if (allowedIPRange == null)
        {
            return NotFound();
        }

        return View(allowedIPRange);
    }

    // POST: AllowedIpRanges/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var allowedIPRange = await context.AllowedIpRanges.FindAsync(id);
        if (allowedIPRange != null)
        {
            context.AllowedIpRanges.Remove(allowedIPRange);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AllowedIpRangeExists(int id) => context.AllowedIpRanges.Any(e => e.Id == id);
}
