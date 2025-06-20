using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;
using System.Security.Claims;
using DodjelaStanovaZG.Helpers.IServices;

namespace DodjelaStanovaZG.Helpers;

public class AuditService(IHttpContextAccessor accessor) : IAuditService
{
    private readonly string _userId = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                                      ?? throw new InvalidOperationException("Korisnik nije prijavljen.");

    public void ApplyAudit(object entity, bool isCreate)
    {
        if (entity is not AuditableEntity auditable) return;

        var now = DateTime.UtcNow;

        if (isCreate)
        {
            auditable.CreatedAt = now;
            auditable.CreatedBy = _userId;
        }

        auditable.UpdatedAt = now;
        auditable.UpdatedBy = _userId;
    }

    public void ApplyAudit(object?[] entities, bool isCreate)
    {
        foreach (var entity in entities)
            if (entity != null)
                ApplyAudit(entity, isCreate);
    }
}