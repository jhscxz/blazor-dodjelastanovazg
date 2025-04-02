using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajBodovniPodaci
{
    [Key]
    public long Id { get; set; }

    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; set; }

    public SocijalniNatjecajZahtjev Zahtjev { get; set; } = null!;

    [Precision(10, 2)]
    public decimal? UkupniPrihodKucanstva { get; set; }

    [Range(0, 10)]
    public byte? BrojGodinaPrebivanja { get; set; }

    public StambeniStatusKucanstva? StambeniStatusKucanstva { get; set; }

    public SastavKucanstva? SastavKucanstva { get; set; }

    public byte? BrojMaloljetneDjece { get; set; }
    public byte? BrojUzdrzavanePunoljetneDjece { get; set; }

    public byte? BrojMaloljetnihKorisnikaInvalidnine { get; set; }
    public byte? BrojOdraslihKorisnikaInvalidnine { get; set; }

    public byte? ZrtvaObiteljskogNasilja { get; set; }
    public byte? BrojOsobaUAlternativnojSkrbi { get; set; }

    [Range(0, 20)]
    public byte? BrojMjeseciObranaSuvereniteta { get; set; }

    public byte? BrojClanovaZrtavaSeksualnogNasiljaDomovinskiRat { get; set; }
    public byte? BrojCivilnihStradalnika { get; set; }

    public byte? ManjeOd35Godina { get; set; }
    public byte? ObrazovanjeBaccMaster { get; set; }

    [MaxLength(450)]
    public string? EditedBy { get; set; }
    public IdentityUser? EditedByUser { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}