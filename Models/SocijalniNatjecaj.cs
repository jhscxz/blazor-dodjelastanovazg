using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecaj
{
    [Key]
    public long Id { get; set; }

    [Required]
    public int KlasaPredmeta { get; set; }

    [Required]
    public DateOnly DatumPodnosenjaZahtjeva { get; set; }

    public string? Adresa { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal UkupniPrihodKucanstva { get; set; }

    [Required]
    public byte StambeniStatusKucanstva { get; set; }

    [Required]
    public byte SastavKucanstva { get; set; }

    public byte Aktivan { get; set; } = 1;

    // FK na ASP.NET IdentityUser (string ID)
    [ForeignKey("CreatedByUser")]
    public string? CreatedBy { get; set; }

    [ForeignKey("EditedByUser")]
    public string? EditedBy { get; set; }

    [Required]
    [ForeignKey("Natjecaj")]
    public long NatjecajId { get; set; }

    public Natjecaj? Natjecaj { get; set; }
    public IdentityUser? CreatedByUser { get; set; }
    public IdentityUser? EditedByUser { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}