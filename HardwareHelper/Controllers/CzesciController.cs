using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HardwareHelper.Models;
using HardwareHelper.Data;

[Authorize(Roles = "Admin, Serwisant")]
public class CzesciController : Controller
{
    private readonly ApplicationDbContext _context;

    public CzesciController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CZESCZAMIENNAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.CzescZamienne.ToListAsync());
    }

    // GET: CZESCZAMIENNAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var czesczamienna = await _context.CzescZamienne
            .FirstOrDefaultAsync(m => m.Id == id);
        if (czesczamienna == null)
        {
            return NotFound();
        }

        return View(czesczamienna);
    }

    // GET: CZESCZAMIENNAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CZESCZAMIENNAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nazwa,Cena,Zlecenia")] CzescZamienna czesczamienna)
    {
        if (ModelState.IsValid)
        {
            _context.Add(czesczamienna);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(czesczamienna);
    }

    // GET: CZESCZAMIENNAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var czesczamienna = await _context.CzescZamienne.FindAsync(id);
        if (czesczamienna == null)
        {
            return NotFound();
        }
        return View(czesczamienna);
    }

    // POST: CZESCZAMIENNAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nazwa,Cena,Zlecenia")] CzescZamienna czesczamienna)
    {
        if (id != czesczamienna.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(czesczamienna);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CzescZamiennaExists(czesczamienna.Id))
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
        return View(czesczamienna);
    }

    // GET: CZESCZAMIENNAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var czesczamienna = await _context.CzescZamienne
            .FirstOrDefaultAsync(m => m.Id == id);
        if (czesczamienna == null)
        {
            return NotFound();
        }

        return View(czesczamienna);
    }

    // POST: CZESCZAMIENNAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var czesczamienna = await _context.CzescZamienne.FindAsync(id);
        if (czesczamienna != null)
        {
            _context.CzescZamienne.Remove(czesczamienna);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CzescZamiennaExists(int? id)
    {
        return _context.CzescZamienne.Any(e => e.Id == id);
    }
}
