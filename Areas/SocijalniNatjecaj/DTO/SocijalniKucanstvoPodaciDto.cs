using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO
{
    public class SocijalniKucanstvoPodaciDto
    {
        public long ZahtjevId { get; set; }

        [Required(ErrorMessage = "Ukupni prihod kućanstva je obavezan.")]
        [Range(0.01, 9999999.99, ErrorMessage = "Iznos mora biti veći od 0.")]
        public decimal? UkupniPrihodKucanstva { get; set; }


        [Required(ErrorMessage = "Datum početka prebivanja je obavezan.")]
        public DateOnly? PrebivanjeOd { get; set; }

        [Required(ErrorMessage = "Stambeni status kućanstva je obavezan.")]
        public StambeniStatusKucanstva? StambeniStatusKucanstva { get; set; }

        [Required(ErrorMessage = "Sastav kućanstva je obavezan.")]
        public SastavKucanstva? SastavKucanstva { get; set; }
        
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedByUserName { get; set; }
        public string? UpdatedByUserName { get; set; }

    }
}