using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Models.Interfaces;

public interface IAuditable
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public IdentityUser? CreatedByUser { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public IdentityUser? UpdatedByUser { get; set; }
}