using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Areas.Profile.DTO;

public class ProfileDto
{
    public string Id { get; set; } = "";

    [Required(ErrorMessage = "Ime je obavezno")]
    public string UserName { get; init; } = "";

    [Required(ErrorMessage = "Email je obavezan")]
    [EmailAddress(ErrorMessage = "Email nije ispravan")]
    public string Email { get; init; } = "";
}