using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajBodovi : AuditableEntity
{
    [Key]
    public long Id { get; init; }
    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; init; }
    public SocijalniNatjecajZahtjev Zahtjev { get; set; } = null!;
    public byte BodoviStambeniStatus { get; set; }
    public byte BodoviSastavKucanstva { get; set; }
    public byte BodoviPoClanu { get; set; }
    public byte BodoviMaloljetni { get; set; }
    public byte BodoviPunoljetniUzdrzavani { get; set; }
    public byte BodoviZajamcenaNaknada { get; set; }
    public byte BodoviNjegovatelj { get; set; }
    public byte BodoviDoplatakZaNjegu { get; set; }
    public byte BodoviOdraslihInvalidnina { get; set; }
    public byte BodoviMaloljetnihInvalidnina { get; set; }
    public byte BodoviZrtvaNasilja { get; set; }
    public byte BodoviAlternativnaSkrb { get; set; }
    public byte BodoviIznad55 { get; set; }
    public float BodoviObrana { get; set; }
    public byte BodoviSeksualnoNasilje { get; set; }
    public byte BodoviCivilniStradalnici { get; set; }
    public float UkupnoBodova { get; set; }
}