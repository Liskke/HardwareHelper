using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HardwareHelper.Models;

namespace HardwareHelper.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Zlecenie> Zlecenia { get; set; }
        public DbSet<Wiadomosc> Wiadomosci { get; set; }
        public DbSet<CzescZamienna> CzescZamienne { get; set; }
    }
}