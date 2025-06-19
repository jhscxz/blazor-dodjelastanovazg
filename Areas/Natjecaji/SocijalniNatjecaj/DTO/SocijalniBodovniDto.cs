using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

public class SocijalniBodovniDto
{
    [Required(ErrorMessage = "Ukupni prihod kućanstva je obavezan.")]
    public decimal? UkupniPrihodKucanstva { get; init; }
    public StambeniStatusKucanstva? StambeniStatusKucanstva { get; init; }
    public SastavKucanstva? SastavKucanstva { get; init; }
}