using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajClan
{
    [Key]
    public long Id { get; set; }

    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; set; }

    public SocijalniNatjecajZahtjev? Zahtjev { get; set; }

    [Required]
    [StringLength(255)]
    public string ImePrezime { get; set; } = string.Empty;

    [Required]
    [StringLength(11)]
    public string Oib { get; set; } = string.Empty;

    [Required]
    public Srodstvo Srodstvo { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}