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

        builder.Entity<IdentityUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        builder.Entity<SocijalniNatjecajZahtjev>(entity =>
        {
            entity.ToTable("SocijalniNatjecajZahtjevi", tb => tb.IsTemporal(temporal =>
            {
                temporal.HasPeriodStart("PeriodStart");
                temporal.HasPeriodEnd("PeriodEnd");
                temporal.UseHistoryTable("SocijalniNatjecajZahtjeviHistory");
            }));
        });

        builder.Entity<SocijalniNatjecajClan>(entity =>
        {
            entity.ToTable("SocijalniNatjecajClanovi", tb => tb.IsTemporal(temporal =>
            {
                temporal.HasPeriodStart("PeriodStart");
                temporal.HasPeriodEnd("PeriodEnd");
                temporal.UseHistoryTable("SocijalniNatjecajClanoviHistory");
            }));
        });

        builder.Entity<SocijalniNatjecajBodovniPodaci>(entity =>
        {
            entity.ToTable("SocijalniNatjecajBodovniPodaci", tb => tb.IsTemporal(temporal =>
            {
                temporal.HasPeriodStart("PeriodStart");
                temporal.HasPeriodEnd("PeriodEnd");
                temporal.UseHistoryTable("SocijalniNatjecajBodovniPodaciHistory");
            }));
        });
    }


    public DbSet<Natjecaj> Natjecaji { get; set; }
    public DbSet<SocijalniNatjecajZahtjev> SocijalniNatjecajZahtjevi { get; set; }
    public DbSet<SocijalniNatjecajClan> SocijalniNatjecajClanovi { get; set; }
    public DbSet<SocijalniNatjecajBodovniPodaci> SocijalniNatjecajBodovniPodaci { get; set; }


}