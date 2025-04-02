using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajZahtjev
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
    public StambeniStatusKucanstva StambeniStatusKucanstva { get; set; }

    [Required]
    public SastavKucanstva SastavKucanstva { get; set; }

    [Required]
    public bool ImaUseljivuNekretninu { get; set; }

    [Required]
    [Range(0, 99)]
    public byte BrojGodinaPrebivanja { get; set; }

    public byte Aktivan { get; set; } = 1;

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

    public ICollection<SocijalniNatjecajClan> Clanovi { get; set; } = [];
    public ICollection<SocijalniKucanstvoDokumentacija> KucanstvoDokumenti { get; set; } = [];

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}