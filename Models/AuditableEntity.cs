using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Models;

public abstract class AuditableEntity
{
    [Required] public DateTime CreatedAt { get; set; }
    [MaxLength(450)] public string? CreatedBy { get; set; }
    [ForeignKey(nameof(CreatedBy))] public IdentityUser? CreatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [MaxLength(450)] public string? UpdatedBy { get; set; }
    [ForeignKey(nameof(UpdatedBy))] public IdentityUser? UpdatedByUser { get; set; }
    [Timestamp]
    [Column(TypeName = "rowversion")]
    public byte[]? RowVersion { get; set; }
}