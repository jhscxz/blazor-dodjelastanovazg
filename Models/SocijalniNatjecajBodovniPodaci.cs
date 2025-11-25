using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajBodovniPodaci : AuditableEntity
{
    [Key]
    public long Id { get; init; }
    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; init; }
    public SocijalniNatjecajZahtjev Zahtjev { get; set; } = null!;

    // Sastav kućanstva
    public byte BrojUzdrzavanePunoljetneDjece { get; set; }

    // Socijalno-zdravstveni status
    public bool PrimateljZajamceneMinimalneNaknade { get; set; }
    public bool StatusRoditeljaNjegovatelja { get; set; }
    public bool KorisnikDoplatkaZaPomoc { get; set; }
    public byte BrojOdraslihKorisnikaInvalidnine { get; set; }
    public byte BrojMaloljetnihKorisnikaInvalidnine { get; set; }

    // Posebne okolnosti
    public bool ZrtvaObiteljskogNasilja { get; set; }
    public byte BrojOsobaUAlternativnojSkrbi { get; set; } // 18–29 godina
    public byte BrojMjeseciObranaSuvereniteta { get; set; }
    public byte BrojClanovaZrtavaSeksualnogNasilja { get; set; }
    public byte BrojCivilnihStradalnika { get; set; }   
}