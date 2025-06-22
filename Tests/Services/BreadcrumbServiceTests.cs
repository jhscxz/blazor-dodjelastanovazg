using DodjelaStanovaZG.Services;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class BreadcrumbServiceTests
{
    [Fact]
    public void Create_UnknownLabel_LogsWarning()
    {
        // Arrange
        var logger = new Mock<ILogger<BreadcrumbService>>();
        var service = new BreadcrumbService(logger.Object);

        // Act
        service.Create("Nepoznato");

        // Assert
        logger.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Nepoznato")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }
}