using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Helpers;

public static class AuditHelper
{
    public static void ApplyAudit(object entity, string userId, bool isCreate)
    {
        if (entity is AuditableEntity auditable)
        {
            var now = DateTime.UtcNow;

            if (isCreate)
            {
                auditable.CreatedAt = now;
                auditable.CreatedBy = userId;
            }

            auditable.UpdatedAt = now;
            auditable.UpdatedBy = userId;
        }
    }
}