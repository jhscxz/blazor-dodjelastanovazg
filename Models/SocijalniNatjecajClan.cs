using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajClan
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string ImeIPrezime { get; set; } = string.Empty;

    [Required]
    public Srodstvo Srodstvo { get; set; }

    [Required]
    [StringLength(11)]
    public string Oib { get; set; } = string.Empty;

    public DateOnly? DatumRodenja { get; set; }

    // Adresa je potrebna samo za podnositelja zahtjeva
    public string? Adresa { get; set; }

    [Required]
    public long NatjecajId { get; set; }

    [ForeignKey(nameof(NatjecajId))]
    public SocijalniNatjecaj? Natjecaj { get; set; }

    public ICollection<DostavljenaDokumentacijaClan> Dokumenti { get; set; } = [];

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
