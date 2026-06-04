using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HardwareHelper.Data;
using HardwareHelper.Models;
using System;
using System.Threading.Tasks;

namespace HardwareHelper.Controllers
{
    [Authorize] // Wymagamy zalogowania, aby w ogóle wejść na ten kontroler
    public class ZleceniaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZleceniaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Zlecenia/Index (Moje Zgłoszenia)
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var mojeZlecenia = await _context.Set<Zlecenie>()
                .Where(z => z.UserId == userId)
                .OrderByDescending(z => z.DataUtworzenia)
                .ToListAsync();

            return View(mojeZlecenia);
        }

        // GET: Zlecenia/Details/ (Szczegóły konkretnego zgłoszenia)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var zlecenie = await _context.Set<Zlecenie>()
                .Include(z => z.Wiadomosc)
                .ThenInclude(w => w.Nadawca)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (zlecenie == null)
            {
                return NotFound();
            }

            return View(zlecenie);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Zlecenia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajWiadomosc(int zlecenieId, string tresc)
        {
            // Jeśli ktoś wyśle puste pole, po prostu odświeżamy stronę
            if (string.IsNullOrWhiteSpace(tresc))
            {
                return RedirectToAction(nameof(Details), new { id = zlecenieId });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Sprawdzamy, czy to zlecenie naprawdę należy do tego klienta
            var zlecenie = await _context.Set<Zlecenie>()
                .FirstOrDefaultAsync(z => z.Id == zlecenieId && z.UserId == userId);

            if (zlecenie == null)
            {
                return NotFound();
            }

            // Tworzymy nową wiadomość z danymi z formularza
            var wiadomosc = new Wiadomosc
            {
                Tresc = tresc,
                ZlecenieId = zlecenieId,
                NadawcaId = userId,
                DataWyslania = DateTime.Now
            };

            _context.Add(wiadomosc);
            await _context.SaveChangesAsync();

            // Po zapisaniu wracamy do widoku szczegółów tego konkretnego zlecenia
            return RedirectToAction(nameof(Details), new { id = zlecenieId });
        }
    }
}