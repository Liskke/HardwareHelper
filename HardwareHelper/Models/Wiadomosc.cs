using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HardwareHelper.Models
{
    public class Wiadomosc
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Treść wiadomości nie może być pusta.")]
        [Display(Name = "Treść")]
        public string Tresc { get; set; }

        [Display(Name = "Data wysłania")]
        public DateTime DataWyslania { get; set; } = DateTime.Now;

        //Relacja 1:N (Jedno zgłoszenie -> Wiele wiadomości)
        public int ZlecenieId { get; set; }
        public Zlecenie? Zlecenie  { get; set; }

        //Relacja 1:N (Jeden nadawca -> Wiele wiadomości)
        public string? NadawcaId { get; set; }
        public IdentityUser? Nadawca { get; set; }
    }
}
