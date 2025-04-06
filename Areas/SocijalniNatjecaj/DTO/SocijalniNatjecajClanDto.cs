using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO
{
    public class SocijalniNatjecajClanDto
    {
        public long Id { get; init; }
        public string ImePrezime { get; set; } = string.Empty;
        public string? Oib { get; set; }
        [Required(ErrorMessage = "Srodstvo je obavezno.")]
        public Srodstvo? Srodstvo { get; set; }
        [Required(ErrorMessage = "Datum rođenja je obavezan.")]
        public DateOnly DatumRodjenja { get; set; }
    }
}