using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HardwareHelper.Data;
using HardwareHelper.Models;
using System.Security.Claims;

namespace HardwareHelper.Controllers
{
    [Authorize(Roles = "Admin,Serwisant")]
    public class SerwisantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SerwisantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Widok główny + wyszukiwanie i filtrowanie
        public async Task<IActionResult> Index(string searchString, int? statusFilter)
        {
            var zlecenia = _context.Zlecenia.Include(z => z.User).AsQueryable();

            // Filtrowanie po numerze naprawy (Id), modelu lub producencie
            if (!string.IsNullOrEmpty(searchString))
            {
                zlecenia = zlecenia.Where(z => z.Model.Contains(searchString)
                                            || z.Producent.Contains(searchString)
                                            || z.Id.ToString() == searchString);
            }

            // Filtrowanie po statusie
            if (statusFilter.HasValue)
            {
                zlecenia = zlecenia.Where(z => (int)z.Status == statusFilter.Value);
            }

            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentStatus"] = statusFilter;
            return View(await zlecenia.ToListAsync());
        }

        // Pełny podgląd zgłoszenia (Read-only)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var zlecenie = await _context.Zlecenia
                .Include(z => z.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zlecenie == null) return NotFound();

            return View(zlecenie);
        }

        // Edycja i zarządzanie zleceniem (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var zlecenie = await _context.Zlecenia.FindAsync(id);
            if (zlecenie == null) return NotFound();

            return View(zlecenie);
        }

        // Edycja i zarządzanie zleceniem (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypUrzadzenia,Producent,Model,NumerSeryjny,Status,ServiceNotes,UserId,PrzewidywanaDataZakonczenia")] Zlecenie uaktualnioneZlecenie)
        {
            if (id != uaktualnioneZlecenie.Id) return NotFound();

            // Usuwamy z walidacji pola klienta, których serwisant nie wypełnia w tym formularzu,
            // zapobiegnie to błędowi niedziałającego zapisu statusu (ModelState.IsValid będzie true)
            ModelState.Remove("OpisUsterki");
            ModelState.Remove("KodPocztowy");
            ModelState.Remove("Miasto");
            ModelState.Remove("Ulica");
            ModelState.Remove("Numer");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    // Pobieramy oryginalny rekord z bazy danych
                    var oryginalneZlecenie = await _context.Zlecenia.FindAsync(id);
                    if (oryginalneZlecenie == null) return NotFound();

                    // Aktualizujemy tylko te pola, które serwisant modyfikuje
                    oryginalneZlecenie.NumerSeryjny = uaktualnioneZlecenie.NumerSeryjny;
                    oryginalneZlecenie.Status = uaktualnioneZlecenie.Status;
                    oryginalneZlecenie.ServiceNotes = uaktualnioneZlecenie.ServiceNotes;
                    oryginalneZlecenie.Model = uaktualnioneZlecenie.Model;
                    oryginalneZlecenie.Producent = uaktualnioneZlecenie.Producent;
                    oryginalneZlecenie.PrzewidywanaDataZakonczenia = uaktualnioneZlecenie.PrzewidywanaDataZakonczenia;

                    _context.Update(oryginalneZlecenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Zlecenia.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(uaktualnioneZlecenie);
        }

        // Moduł komunikacji (Podgląd wiadomości i wysyłanie odpowiedzi)
        public async Task<IActionResult> SzczegolyIKomunikacja(int? id)
        {
            if (id == null) return NotFound();
            var zlecenie = await _context.Zlecenia
                .Include(z => z.Wiadomosc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zlecenie == null) return NotFound();

            return View(zlecenie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajWiadomosc(int zlecenieId, string trescWiadomosci)
        {
            if (string.IsNullOrEmpty(trescWiadomosci))
            {
                return RedirectToAction(nameof(SzczegolyIKomunikacja), new { id = zlecenieId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nowaWiadomosc = new Wiadomosc
            {
                ZlecenieId = zlecenieId,
                Tresc = trescWiadomosci,
                DataWyslania = DateTime.Now,
                NadawcaId = userId
            };

            _context.Wiadomosci.Add(nowaWiadomosc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SzczegolyIKomunikacja), new { id = zlecenieId });
        }
    }
}