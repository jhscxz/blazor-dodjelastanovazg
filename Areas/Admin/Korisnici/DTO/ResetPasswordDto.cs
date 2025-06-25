using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.DTO
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Nova lozinka je obavezna.")]
        [MinLength(6, ErrorMessage = "Lozinka mora imati najmanje 6 znakova.")]
        public string NewPassword { get; set; } = "";
        [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
        [Compare("NewPassword", ErrorMessage = "Lozinke se ne podudaraju.")]
        public string ConfirmPassword { get; set; } = "";
    }
}