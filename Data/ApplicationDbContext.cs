using DodjelaStanovaZG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Identity konfiguracija

        // Osiguraj jedinstvenost korisničkog imena (UserName)
        builder.Entity<IdentityUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        #endregion

        #region Temporal konfiguracije

        // Aktiviraj praćenje vremenske povijesti za glavne entitete (temporal tables)
        
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

        builder.Entity<SocijalniNatjecajKucanstvoPodaci>(entity =>
        {
            entity.ToTable("SocijalniNatjecajKucanstvoPodaci", tb => tb.IsTemporal(temporal =>
            {
                temporal.HasPeriodStart("PeriodStart");
                temporal.HasPeriodEnd("PeriodEnd");
                temporal.UseHistoryTable("SocijalniNatjecajKucanstvoPodaciHistory");
            }));
        });

        #endregion

        #region Veze između entiteta

        // 1:1 veza između SocijalniPrihodi i SocijalniNatjecajKucanstvoPodaci putem shared primary key
        builder.Entity<SocijalniPrihodi>()
            .HasOne(p => p.KucanstvoPodaci)
            .WithOne(k => k.Prihod)
            .HasForeignKey<SocijalniPrihodi>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
    }

    #region DbSet-ovi

    public DbSet<Natjecaj> Natjecaji { get; set; }
    public DbSet<SocijalniNatjecajZahtjev> SocijalniNatjecajZahtjevi { get; set; }
    public DbSet<SocijalniNatjecajClan> SocijalniNatjecajClanovi { get; set; }
    public DbSet<SocijalniNatjecajBodovniPodaci> SocijalniNatjecajBodovniPodaci { get; set; }
    public DbSet<SocijalniNatjecajKucanstvoPodaci> SocijalniNatjecajKucanstvoPodaci { get; set; }
    public DbSet<SocijalniPrihodi> SocijalniPrihodi { get; set; }
    public DbSet<SocijalniNatjecajBodovi> SocijalniNatjecajBodovi { get; set; }
    public DbSet<SocijalniNatjecajBodovnaGreska> SocijalniNatjecajBodovnaGreske { get; set; }

    #endregion
}
