using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajBodovniPodaci : AuditableEntity
{
    [Key]
    public long Id { get; set; }

    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; set; }

    public SocijalniNatjecajZahtjev Zahtjev { get; set; } = null!;

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
}