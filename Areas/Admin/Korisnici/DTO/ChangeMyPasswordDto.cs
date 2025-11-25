using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.DTO
{
    public class ChangeMyPasswordDto
    {
        [Required(ErrorMessage = "Stara lozinka je obavezna")]
        public string OldPassword { get; set; } = "";
        [Required(ErrorMessage = "Nova lozinka je obavezna")]
        public string NewPassword { get; set; } = "";
        [Required(ErrorMessage = "Potvrda nove lozinke je obavezna")]
        [Compare("NewPassword", ErrorMessage = "Lozinke se ne podudaraju")]
        public string ConfirmPassword { get; set; } = "";
    }
}