namespace DodjelaStanovaZG.Helpers.IServices;

public interface IAuditService
{
    void ApplyAudit(object entity, bool isCreate);
    void ApplyAudit(object?[] entities, bool isCreate);
}