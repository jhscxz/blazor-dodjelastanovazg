using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class SocijalniBodovniDto
{
    [Required(ErrorMessage = "Ukupni prihod kućanstva je obavezan.")]
    public decimal? UkupniPrihodKucanstva { get; set; }
    public StambeniStatusKucanstva? StambeniStatusKucanstva { get; set; }
    public SastavKucanstva? SastavKucanstva { get; set; }
}