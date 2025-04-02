using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DodjelaStanovaZG.Models;

public class SocijalniKucanstvoDokumentacija
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long NatjecajZahtjevId { get; set; }

    [Required]
    public int DokazniDokumentId { get; set; }

    [Required]
    public bool Dostavljeno { get; set; }

    [ForeignKey(nameof(NatjecajZahtjevId))]
    public SocijalniNatjecajZahtjev? NatjecajZahtjev { get; set; }

    [ForeignKey(nameof(DokazniDokumentId))]
    public DokazniDokument? DokazniDokument { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}