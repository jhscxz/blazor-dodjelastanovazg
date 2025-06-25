using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;

public class AddUserDto
{
    [Required(ErrorMessage = "Korisničko ime je obavezno")]
    public string UserName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email je obavezan")]
    [EmailAddress(ErrorMessage = "Email nije ispravan")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Lozinka je obavezna")]
    public string Password { get; set; } = string.Empty;

}