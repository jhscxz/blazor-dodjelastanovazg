using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DodjelaStanovaZG.Models;

public class DostavljenaDokumentacijaClan
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long ClanId { get; set; }

    [ForeignKey(nameof(ClanId))]
    public SocijalniNatjecajClan? Clan { get; set; }

    [Required]
    public int DokazniDokumentId { get; set; }

    [ForeignKey(nameof(DokazniDokumentId))]
    public DokazniDokument? Dokument { get; set; }

    public bool Dostavljeno { get; set; } = false;
    public string? Napomena { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}