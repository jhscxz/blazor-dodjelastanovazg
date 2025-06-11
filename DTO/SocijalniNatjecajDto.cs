using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.DTO;

public class SocijalniNatjecajDto
{
    public long? Id { get; set; }

    [Required]
    public int KlasaPredmeta { get; set; }

    [Required]
    public DateOnly DatumPodnosenjaZahtjeva { get; set; }

    public string? Adresa { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal UkupniPrihodKucanstva { get; set; }

    [Required]
    [Range(1, 10)]
    public byte StambeniStatusKucanstva { get; set; }

    [Required]
    [Range(1, 10)]
    public byte SastavKucanstva { get; set; }

    public byte Aktivan { get; set; } = 1;

    public long? CreatedBy { get; set; }
    public long? EditedBy { get; set; }

    [Required]
    public long NatjecajId { get; set; }
}