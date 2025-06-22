using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajClan : AuditableEntity
{
    [Key]
    public long Id { get; set; }
    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; set; }
    public required SocijalniNatjecajZahtjev? Zahtjev { get; set; } = null!;
    [Required]
    [StringLength(255)]
    public string ImePrezime { get; set; } = string.Empty;
    [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB mora sadržavati 11 znamenki.")]
    [StringLength(11)]
    public string? Oib { get; set; } = string.Empty;
    public Srodstvo? Srodstvo { get; set; }
    [Required]
    public DateOnly DatumRodjenja { get; set; }
}