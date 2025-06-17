namespace DodjelaStanovaZG.Helpers.IServices;

public interface IAuditService
{
    void ApplyAudit(object entity, bool isCreate);
    void ApplyAudit(IEnumerable<object> entities, bool isCreate);
}