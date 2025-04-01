namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class SocijalniNatjecajDto
{
    public long Id { get; set; }
    public int KlasaPredmeta { get; set; }
    public DateOnly DatumPodnosenjaZahtjeva { get; set; }
    public string? Adresa { get; set; }
    public decimal UkupniPrihodKucanstva { get; set; }
    public byte StambeniStatusKucanstva { get; set; }
    public byte SastavKucanstva { get; set; }
    public byte Aktivan { get; set; }
    public long NatjecajId { get; set; }
}