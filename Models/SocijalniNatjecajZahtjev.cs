using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajZahtjev : AuditableEntity
{
    [Key] public long Id { get; set; }
    [Required] public int KlasaPredmeta { get; set; }
    [Required] public DateTime DatumPodnosenjaZahtjeva { get; set; }
    [StringLength(255)] public string? Adresa { get; set; }
    [StringLength(255)] [EmailAddress] public string? Email { get; set; }
    public bool PosjedujeNekretninuZg { get; set; } = false;
    [Required]
    public RezultatObrade RezultatObrade { get; set; }
    [Required]
    public RezultatObrade ManualniRezultatObrade { get; set; }
    [StringLength(1000)] public string? NapomenaObrade { get; set; }
    [Required]
    [ForeignKey(nameof(Natjecaj))]
    public long NatjecajId { get;  set; }
    public Natjecaj? Natjecaj { get; set; }
    public ICollection<SocijalniNatjecajClan> Clanovi { get; set; } = new List<SocijalniNatjecajClan>();
    public SocijalniNatjecajKucanstvoPodaci? KucanstvoPodaci { get; set; }
    public SocijalniNatjecajBodovniPodaci? BodovniPodaci { get; set; }
    public SocijalniNatjecajBodovi? Bodovi { get; set; }
    public ICollection<SocijalniNatjecajBodovnaGreska> Greske { get; set; } = new List<SocijalniNatjecajBodovnaGreska>();
}