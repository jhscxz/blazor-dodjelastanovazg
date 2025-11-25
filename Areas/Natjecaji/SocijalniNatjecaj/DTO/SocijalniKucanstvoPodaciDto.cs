using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

public class SocijalniKucanstvoPodaciDto : AuditableDto
{
    public long ZahtjevId { get; init; }
    [Required(ErrorMessage = "Datum početka prebivanja je obavezan.")]
    public DateOnly? PrebivanjeOd { get; set; }
    [Required(ErrorMessage = "Stambeni status kućanstva je obavezan.")]
    public StambeniStatusKucanstva? StambeniStatusKucanstva { get; set; }
    [Required(ErrorMessage = "Sastav kućanstva je obavezan.")]
    public SastavKucanstva? SastavKucanstva { get; set; }
    public SocijalniPrihodDto? Prihod { get; set; }
}