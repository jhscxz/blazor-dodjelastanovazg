namespace DodjelaStanovaZG.Services.Interfaces;

public interface IAuditService
{
    void ApplyAudit(object entity, bool isCreate);
    void ApplyAudit(object?[] entities, bool isCreate);
}