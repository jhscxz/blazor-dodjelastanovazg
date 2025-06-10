using DodjelaStanovaZG.DTO;
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
    
    public static void MapAudit(AuditableEntity entity, AuditableDto dto)
    {
        dto.CreatedAt = entity.CreatedAt;
        dto.CreatedBy = entity.CreatedBy;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.UpdatedBy = entity.UpdatedBy;
        dto.CreatedByUserName = entity.CreatedByUser?.UserName;
        dto.UpdatedByUserName = entity.UpdatedByUser?.UserName;
    }
    
}