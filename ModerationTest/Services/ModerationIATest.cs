using Moderation.DTO;
using Moderation.Interfaces;
using Moq;

namespace Moderation.Services;

[TestFixture]
public class ModerationIATest
{
    [Test]
    public async Task Moderate_ShoudDetectViolenceWhenPresent()
    {
        var mockClient = new Mock<IModerationClient>();
        
        mockClient.Setup(c => c.IsIllicitOrViolentAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        
        var service = new ModerationIA(mockClient.Object);
        
        var result =  await service.Moderate("quero matar pessoas");
        
        Assert.IsTrue(result);
        
    }
    
}