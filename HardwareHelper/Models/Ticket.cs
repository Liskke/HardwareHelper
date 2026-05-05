using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HardwareHelper.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Typ urządzenia jest wymagany.")]
        [Display(Name = "Typ urządzenia")]
        public string DeviceType { get; set; }

        [Required(ErrorMessage = "Producent jest wymagany.")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Model jest wymagany.")]
        public string Model { get; set; }

        [Display(Name = "Numer seryjny")]
        public string? SerialNumber { get; set; }

        [Required(ErrorMessage = "Opis usterki jest wymagany.")]
        [MinLength(10, ErrorMessage = "Opis musi mieć minimum 10 znaków.")]
        [Display(Name = "Opis usterki")]
        public string IssueDescription { get; set; }

        [Display(Name = "Czy dołączono zasilacz?")]
        public bool HasPowerSupply { get; set; }

        [Display(Name = "Zamówić kuriera?")]
        public bool NeedsCourier { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Nowe;

        // Notatki dla serwisanta (np. "Matryca przyklejona na klej zamiast na taśmę dwustronną - konieczny kontakt z klientem")
        [Display(Name = "Notatki techniczne (tylko serwis)")]
        public string? ServiceNotes { get; set; }

        // RELACJA 1:N (Jeden użytkownik -> Wiele zgłoszeń)
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        // RELACJA 1:N (Jedno zgłoszenie -> Wiele wiadomości)
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        // RELACJA N:M (Wiele zgłoszeń -> Wiele różnych części) - Wymóg na 5 pkt
        public ICollection<RepairPart> RepairParts { get; set; } = new List<RepairPart>();
    }
}