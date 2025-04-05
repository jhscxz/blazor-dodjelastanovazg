using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO
{
    public class SocijalniNatjecajDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Klasa predmeta je obavezna.")]
        public int? KlasaPredmeta { get; set; }

        [Required(ErrorMessage = "Datum podnošenja zahtjeva je obavezan.")]
        public DateOnly DatumPodnosenjaZahtjeva { get; set; }

        public string? Adresa { get; set; }

        [Required(ErrorMessage = "Ime i prezime podnositelja su obavezni.")]
        public string ImePrezime { get; set; } = string.Empty;

        public string? Oib { get; set; }

        [Required(ErrorMessage = "Rezultat obrade je obavezan.")]
        public RezultatObrade? RezultatObrade { get; set; }

        public string? NapomenaObrade { get; set; }

        public long NatjecajId { get; set; }

        [Required]
        public SocijalniBodovniDto Bodovni { get; set; } = new();

        // Dodani property za članove kućanstva
        public List<SocijalniNatjecajClanDto>? Clanovi { get; set; }
    }
}