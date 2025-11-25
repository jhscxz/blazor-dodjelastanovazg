using System.Security.Claims;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Services;

public class AuditService(IHttpContextAccessor accessor, ILogger<AuditService> logger)
    : IAuditService
{
    // Dohvaća ID korisnika iz HTTP konteksta; ako nije dostupan, koristi "system"
    private readonly string _userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";

    public void ApplyAudit(object entity, bool isCreate)
    {
        logger.LogDebug("Applying audit on {EntityType} (create: {IsCreate}) by {UserId}", entity.GetType().Name, isCreate, _userId);

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