using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Helpers;

public static class AuditExtensions
{
    public static TDto WithAuditFrom<TEntity, TDto>(this TDto dto, TEntity entity)
        where TEntity : AuditableEntity
        where TDto : AuditableDto
    {
        dto.CreatedAt = entity.CreatedAt;
        dto.CreatedBy = entity.CreatedBy;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.UpdatedBy = entity.UpdatedBy;
        dto.CreatedByUserName = entity.CreatedByUser?.UserName;
        dto.UpdatedByUserName = entity.UpdatedByUser?.UserName;
        return dto;
    }
}