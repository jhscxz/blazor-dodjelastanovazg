using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO
{
    public class SocijalniNatjecajZahtjevDto : AuditableDto
    {
        public long Id { get; init; }
        [Required(ErrorMessage = "Klasa predmeta je obavezna.")]
        public int? KlasaPredmeta { get; set; }
        [Required(ErrorMessage = "Datum podnošenja je obavezan.")]
        public DateTime? DatumPodnosenjaZahtjeva { get; set; }
        public string? Adresa { get; set; }
        [EmailAddress(ErrorMessage = "Unesite važeći email.")]
        [StringLength(255, ErrorMessage = "Email adresa ne smije biti duža od 255 znakova.")]
        public string? Email { get; set; }
        public bool PosjedujeNekretninuZg { get; set; } = false;
        [Required(ErrorMessage = "Ime i prezime podnositelja su obavezni.")]
        public string ImePrezime { get; set; } = string.Empty;
        [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB mora sadržavati 11 znamenki.")]
        [StringLength(11)]
        public string? Oib { get; set; }
        [Required(ErrorMessage = "Rezultat obrade je obavezan.")]
        public RezultatObrade? RezultatObrade { get; set; }
        public string? NapomenaObrade { get; set; }
        public long NatjecajId { get; set; }
        public byte[]? RowVersion { get; set; }
        public SocijalniPrihodDto? Prihod { get; set; }
        public SocijalniNatjecajBodovi? Bodovi { get; set; } 
        public List<SocijalniNatjecajClanDto>? Clanovi { get; set; }

        public SocijalniKucanstvoPodaciDto? KucanstvoPodaci { get; set; }
        public SocijalniNatjecajBodovniPodaciDto? BodovniPodaci { get; set; }
        public byte BrojMaloljetneDjece { get; set; }
        public bool PodnositeljIznad55 { get; set; }
    }
}