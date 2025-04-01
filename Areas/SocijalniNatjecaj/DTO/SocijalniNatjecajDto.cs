namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class SocijalniNatjecajDto
{
    public int KlasaPredmeta { get; init; }
    public DateOnly DatumPodnosenjaZahtjeva { get; init; }
    public string? Adresa { get; init; }
    public decimal UkupniPrihodKucanstva { get; init; }
    public byte StambeniStatusKucanstva { get; init; }
    public byte SastavKucanstva { get; init; }
    public byte Aktivan { get; init; }
    public long NatjecajId { get; init; }
}