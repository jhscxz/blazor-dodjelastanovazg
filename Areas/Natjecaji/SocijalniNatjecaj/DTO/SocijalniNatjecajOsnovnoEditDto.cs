using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO
{
    public class SocijalniNatjecajOsnovnoEditDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Klasa predmeta je obavezna.")]
        public int? KlasaPredmeta { get; set; }
        [Required(ErrorMessage = "Datum podnošenja zahtjeva je obavezan.")]
        public DateTime? DatumPodnosenjaZahtjeva { get; set; }
        public string? Adresa { get; set; }
        [EmailAddress(ErrorMessage = "Unesite važeći email.")]
        [StringLength(255, ErrorMessage = "Email adresa ne smije biti duža od 255 znakova.")]
        public string? Email { get; set; }
        public RezultatObrade? RezultatObrade { get; set; }
        public string? NapomenaObrade { get; set; }
        public long NatjecajId { get; set; }
        
    }
}