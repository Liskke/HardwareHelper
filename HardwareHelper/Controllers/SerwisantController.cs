using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HardwareHelper.Data;
using HardwareHelper.Models;
using System.Security.Claims;

namespace HardwareHelper.Controllers
{
    [Authorize(Roles = "Admin,Serwisant")] // Dostęp tylko dla Serwisanta i Admina
    public class SerwisantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SerwisantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ZADANIE A: Widok główny + wyszukiwanie i filtrowanie
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

            // Filtrowanie po statusie (o ile Status w modelu to int/enum)
            if (statusFilter.HasValue)
            {
                zlecenia = zlecenia.Where(z => (int)z.Status == statusFilter.Value);
            }

            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentStatus"] = statusFilter;

            return View(await zlecenia.ToListAsync());
        }

        // ZADANIE B: Edycja i zarządzanie zleceniem (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var zlecenie = await _context.Zlecenia.FindAsync(id);
            if (zlecenie == null) return NotFound();

            return View(zlecenie);
        }

        // ZADANIE B: Edycja i zarządzanie zleceniem (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypUrzadzenia,Producent,Model,NumerSeryjny,OpisUsterki,CzyJestZasilacz,NaprawaGwarancyjna,DataZakupu,KodPocztowy,Miasto,Ulica,Numer,Status,ServiceNotes,UserId")] Zlecenie statusI_notatki)
        {
            if (id != statusI_notatki.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Pobieramy oryginalny obiekt, aby zaktualizować tylko zmienione przez serwisanta pola
                    var oryginalneZlecenie = await _context.Zlecenia.FindAsync(id);
                    if (oryginalneZlecenie == null) return NotFound();

                    // Aktualizacja danych klienta (w razie literówki), statusu i notatek
                    oryginalneZlecenie.NumerSeryjny = statusI_notatki.NumerSeryjny;
                    oryginalneZlecenie.Status = statusI_notatki.Status;
                    oryginalneZlecenie.ServiceNotes = statusI_notatki.ServiceNotes;

                    // Opcjonalnie: jeśli serwisant może poprawiać inne dane klienta
                    oryginalneZlecenie.Model = statusI_notatki.Model;
                    oryginalneZlecenie.Producent = statusI_notatki.Producent;

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
            return View(statusI_notatki);
        }

        // ZADANIE C: Moduł komunikacji (Podgląd wiadomości i wysyłanie odpowiedzi)
        public async Task<IActionResult> SzczegolyIKomunikacja(int? id)
        {
            if (id == null) return NotFound();

            var zlecenie = await _context.Zlecenia
                .Include(z => z.Wiadomosc) // Zakładam, że kolekcja wiadomości w modelu nazywa się Wiadomosc lub Wiadomosci
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Id zalogowanego serwisanta

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