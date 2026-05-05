using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace HardwareHelper.Models
{
    public enum TypUrzadzenia
    {
        [Display(Name = "Laptop")]
        Laptop,

        [Display(Name = "Komputer PC")]
        PC,

        [Display(Name = "Monitor")]
        Monitor,

        [Display(Name = "Konsola")]
        Konsola,

        [Display(Name = "Zasilacz / Akcesoria")]
        Akcesoria,

        [Display(Name = "Inne")]
        Inne
    }
    public class Zlecenie
    {
        public int Id { get; set; }

        [Display(Name = "Numer Zlecenia")]
        public string? NumerZlecenia { get; set; }

        [Required(ErrorMessage = "Wybierz typ urządzenia.")]
        [Display(Name = "Typ urządzenia")]
        public TypUrzadzenia TypUrzadzenia { get; set; }

        [Required(ErrorMessage = "Producent jest wymagany.")]
        [Display(Name = "Producent")]
        public string Producent { get; set; }

        [Required(ErrorMessage = "Model jest wymagany.")]
        public string Model { get; set; }

        [Display(Name = "Numer seryjny")]
        public string? NumerSeryjny { get; set; }

        [Required(ErrorMessage = "Opis usterki jest wymagany.")]
        [MinLength(10, ErrorMessage = "Opis musi mieć minimum 10 znaków.")]
        [Display(Name = "Opis usterki")]
        public string OpisUsterki { get; set; }

        [Display(Name = "Czy dołączono zasilacz?")]
        public bool CzyJestZasilacz { get; set; }

        [Display(Name = "Naprawa w ramach gwarancji")]
        public bool NaprawaGwarancyjna { get; set; }

        [Display(Name = "Data zakupu")]
        public DateTime? DataZakupu { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string KodPocztowy { get; set; }

        [Display(Name = "Miasto")]
        public string Miasto { get; set; }

        [Display(Name = "Ulica/Osiedle")]
        public string Ulica { get; set; }

        [Display(Name = "Numer")]
        public int Numer { get; set; }

        public ZlecenieStatus Status { get; set; } = ZlecenieStatus.Oczekuje_na_dostarczenie;

        [Display(Name = "Data utworzenia")]
        public DateTime DataUtworzenia { get; set; } = DateTime.Now;

        [Display(Name = "Przewidywana data zakończenia")]
        public DateTime? PrzewidywanaDataZakonczenia { get; set; }

        [Display(Name = "Notatki techniczne (tylko serwis)")]
        public string? ServiceNotes { get; set; }



        //Relacja 1:N (Jeden użytkownik -> Wiele zgłoszeń)
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        //Relacja 1:N (Jedno zgłoszenie -> Wiele wiadomości)
        public ICollection<Wiadomosc> Wiadomosc { get; set; } = new List<Wiadomosc>();

        //Relacja N:M (Wiele zgłoszeń -> Wiele różnych części)
        public ICollection<CzescZamienna> CzescZamienna { get; set; } = new List<CzescZamienna>();
    }
}