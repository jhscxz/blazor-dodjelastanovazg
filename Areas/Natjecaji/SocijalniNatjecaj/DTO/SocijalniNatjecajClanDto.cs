using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO
{
    public class SocijalniNatjecajClanDto : AuditableDto
    {
        public long Id { get; init; }
        public long ZahtjevId { get; init; }
        public string ImePrezime { get; set; } = string.Empty;
        [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB mora sadržavati 11 znamenki.")]
        [StringLength(11)]
        public string? Oib { get; set; }
        [Required(ErrorMessage = "Srodstvo je obavezno.")]
        public Srodstvo? Srodstvo { get; set; }
        [Required(ErrorMessage = "Datum rođenja je obavezan.")]
        public DateOnly DatumRodjenja { get; set; }
    }
}