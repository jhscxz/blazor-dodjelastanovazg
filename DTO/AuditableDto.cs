namespace DodjelaStanovaZG.DTO;

public abstract class AuditableDto
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string? CreatedByUserName { get; set; }
    public string? UpdatedByUserName { get; set; }
}