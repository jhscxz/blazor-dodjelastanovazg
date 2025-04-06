using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajZahtjev
{
    [Key] public long Id { get; set; }
    [Required] public int KlasaPredmeta { get; set; }
    [Required] public DateOnly DatumPodnosenjaZahtjeva { get; set; }
    [StringLength(255)] public string? Adresa { get; set; }
    [StringLength(255)] [EmailAddress] public string? Email { get; set; }
    [Required] public RezultatObrade RezultatObrade { get; set; }
    [StringLength(1000)] public string? NapomenaObrade { get; set; }
    [ForeignKey(nameof(CreatedByUser))]
    [MaxLength(450)]
    public string? CreatedBy { get; set; }
    public IdentityUser? CreatedByUser { get; set; }
    [Required]
    [ForeignKey(nameof(Natjecaj))]
    public long NatjecajId { get; set; }
    public Natjecaj? Natjecaj { get; set; }
    public ICollection<SocijalniNatjecajClan> Clanovi { get; set; } = new List<SocijalniNatjecajClan>();
    public SocijalniNatjecajKucanstvoPodaci? KucanstvoPodaci { get; set; }
    public SocijalniNatjecajBodovniPodaci? BodovniPodaci { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}