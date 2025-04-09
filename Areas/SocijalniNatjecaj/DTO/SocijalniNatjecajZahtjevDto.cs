using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO
{
    public class SocijalniNatjecajZahtjevDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Klasa predmeta je obavezna.")]
        public int? KlasaPredmeta { get; set; }
        [Required(ErrorMessage = "Datum podnošenja je obavezan.")]
        public DateTime? DatumPodnosenjaZahtjeva { get; set; }
        public string? Adresa { get; set; }
        [EmailAddress(ErrorMessage = "Unesite važeći email.")]
        [StringLength(255, ErrorMessage = "Email adresa ne smije biti duža od 255 znakova.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Ime i prezime podnositelja su obavezni.")]
        public string ImePrezime { get; set; } = string.Empty;
        public string? Oib { get; set; }
        [Required(ErrorMessage = "Rezultat obrade je obavezan.")]
        public RezultatObrade? RezultatObrade { get; set; }
        public string? NapomenaObrade { get; set; }
        public long NatjecajId { get; set; }
        [Required]
        public SocijalniBodovniDto Bodovni { get; set; } = new();
        public List<SocijalniNatjecajClanDto>? Clanovi { get; set; }
        public SocijalniKucanstvoPodaciDto? KucanstvoPodaci { get; set; }
    }
}