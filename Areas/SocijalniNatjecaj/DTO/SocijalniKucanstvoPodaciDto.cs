using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO
{
    public class SocijalniKucanstvoPodaciDto : AuditableDto
    {
        public long ZahtjevId { get; init; }

        [Required(ErrorMessage = "Ukupni prihod kućanstva je obavezan.")]
        [Range(0.01, 9999999.99, ErrorMessage = "Iznos mora biti veći od 0.")]
        public decimal? UkupniPrihodKucanstva { get; set; }

        // ➕ Mjesečni prihod po članu
        public decimal? PrihodPoClanu { get; set; }

        // ➕ Postotak u odnosu na prosjek plaće
        public decimal? PostotakProsjeka { get; set; }

        // ➕ Da li zadovoljava uvjet prihoda?
        public bool? IspunjavaUvjetPrihoda { get; set; }

        [Required(ErrorMessage = "Datum početka prebivanja je obavezan.")]
        public DateOnly? PrebivanjeOd { get; set; }

        [Required(ErrorMessage = "Stambeni status kućanstva je obavezan.")]
        public StambeniStatusKucanstva? StambeniStatusKucanstva { get; set; }

        [Required(ErrorMessage = "Sastav kućanstva je obavezan.")]
        public SastavKucanstva? SastavKucanstva { get; set; }
    }
}