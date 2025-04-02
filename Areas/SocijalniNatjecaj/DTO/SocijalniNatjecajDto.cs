using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class SocijalniNatjecajDto
{
    [Required(ErrorMessage = "Klasa predmeta je obavezna.")]
    public int? KlasaPredmeta { get; set; } 
    [Required(ErrorMessage = "Datum podnošenja zahtjeva je obavezan.")]
    public DateOnly DatumPodnosenjaZahtjeva { get; set; }
    [Required(ErrorMessage = "Adresa je obavezna.")]
    public string Adresa { get; set; } = string.Empty;
    [Required(ErrorMessage = "Ukupni prihod kućanstva je obavezan.")]
    public decimal? UkupniPrihodKucanstva { get; set; }
    [Required(ErrorMessage = "Stambeni status kućanstva je obavezan.")]
    public StambeniStatusKucanstva? StambeniStatusKucanstva { get; set; }
    [Required(ErrorMessage = "Sastav kućanstva je obavezan.")]
    public SastavKucanstva? SastavKucanstva { get; set; }
    public byte Aktivan { get; set; } = 1;
    public long NatjecajId { get; set; }
}