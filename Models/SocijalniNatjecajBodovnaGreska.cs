using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DodjelaStanovaZG.Models;

public class SocijalniNatjecajBodovnaGreska : AuditableEntity
{
    [Key] public long Id { get; set; }
    [Required]
    [ForeignKey(nameof(Zahtjev))]
    public long ZahtjevId { get; init; }
    [Required] [StringLength(50)] public string Kod { get; set; } = null!;
    [Required] [StringLength(500)] public string Poruka { get; set; } = null!;
    public SocijalniNatjecajZahtjev Zahtjev { get; init; } = null!;
}