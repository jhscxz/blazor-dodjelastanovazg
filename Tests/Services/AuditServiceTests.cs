using System.Security.Claims;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class AuditServiceTests
{
    private class TestEntity : AuditableEntity;

    private static AuditService CreateService(string userId)
    {
        var accessor = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userId)
            ]))
        };
        accessor.Setup(a => a.HttpContext).Returns(context);

        var logger = new Mock<ILogger<AuditService>>();
        return new AuditService(accessor.Object, logger.Object);
    }

    [Fact]
    public void ApplyAudit_Create_SetsCreatedAndUpdated()
    {
        var entity = new TestEntity();
        var service = CreateService("test-user");

        service.ApplyAudit(entity, true);

        Assert.Equal("test-user", entity.CreatedBy);
        Assert.Equal("test-user", entity.UpdatedBy);
        Assert.NotEqual(default, entity.CreatedAt);
        Assert.Equal(entity.CreatedAt, entity.UpdatedAt);
    }

    [Fact]
    public void ApplyAudit_Update_OnlyUpdatesUpdatedFields()
    {
        var createdAt = new DateTime(2020, 1, 1);
        var entity = new TestEntity
        {
            CreatedAt = createdAt,
            CreatedBy = "initial",
            UpdatedAt = createdAt,
            UpdatedBy = "initial"
        };
        var service = CreateService("editor");

        service.ApplyAudit(entity, false);

        Assert.Equal(createdAt, entity.CreatedAt);
        Assert.Equal("initial", entity.CreatedBy);
        Assert.NotEqual(createdAt, entity.UpdatedAt);
        Assert.Equal("editor", entity.UpdatedBy);
    }
}