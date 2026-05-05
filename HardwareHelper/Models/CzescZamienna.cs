using System.ComponentModel.DataAnnotations;
using static System.Net.WebRequestMethods;

namespace HardwareHelper.Models
{
    public class CzescZamienna
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa części jest wymagana.")]
        [Display(Name = "Nazwa części")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Podaj cenę części.")]
        [Display(Name = "Cena (PLN)")]
        public decimal Cena { get; set; }

        //Relacja N:M (Wiele części -> Wiele różnych zgłoszeń)
        public ICollection<Zlecenie> Zlecenia { get; set; } = new List<Zlecenie>();
    }
}