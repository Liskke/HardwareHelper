using System.ComponentModel.DataAnnotations;
namespace HardwareHelper.Models
{
    public enum ZlecenieStatus
    {
        [Display(Name = "Oczekuje na dostarczenie")]
        Oczekuje_na_dostarczenie,

        [Display(Name = "Oczekuje na diagnozę")]
        Oczekuje_na_diagnose,

        [Display(Name = "W trakcie diagnozy")]
        W_trakcie_diagnozy,

        [Display(Name = "W trakcie naprawy")]
        W_trakcie_naprawy,

        [Display(Name = "Oczekuje na części")]
        Oczekuje_na_czesci,

        [Display(Name = "W trakcie testów")]
        W_trakcie_testow,

        [Display(Name = "Zakończone")]
        Zakonczone,
    }
}
