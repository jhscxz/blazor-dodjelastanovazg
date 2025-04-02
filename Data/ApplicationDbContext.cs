using DodjelaStanovaZG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityUser>().HasIndex(u => u.UserName).IsUnique();
    }
    
    public DbSet<Natjecaj> Natjecaji { get; set; }
    public DbSet<SocijalniNatjecajZahtjev> SocijalniNatjecajZahtjevi { get; set; }
    public DbSet<SocijalniKucanstvoDokumentacija> SocijalniKucanstvoDokumenti { get; set; }
    // Socijalni natječaj - članovi i dokumentacija
    public DbSet<SocijalniNatjecajClan> SocijalniNatjecajClanovi { get; set; }
    public DbSet<DokazniDokument> DokazniDokumenti { get; set; }
    public DbSet<DostavljenaDokumentacijaClan> DostavljenaDokumentacijaClanova { get; set; }


}