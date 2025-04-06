using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajClan
{
    [Key]
    public long Id { get; init; }
    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; init; }
    public required SocijalniNatjecajZahtjev Zahtjev { get; init; } = null!;
    [Required]
    [StringLength(255)]
    public string ImePrezime { get; init; } = string.Empty;
    [StringLength(11)]
    public string? Oib { get; init; } = string.Empty;
    public Srodstvo? Srodstvo { get; init; }
    [Required]
    public DateOnly DatumRodjenja { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}