using System.ClientModel;
using System.ClientModel.Primitives;
using Moderation.DTO;
using Moderation.Interfaces;
using Moq;
using OpenAI.Moderations;

namespace Moderation.Services;

[TestFixture]
public class ModerationIATest
{
    [Test]
    public void Moderate_ShoudDetectViolenceWhenPresent()
    {
        var mockClient = new Mock<IModerationClient>();
        
        var fakeResponse = new ModerationResultDto(
            Flagged: true,
            Violence: true,
            SelfHarm: false,
            Sexual: false,
            Hate: false
        );
        
        mockClient.Setup(c => c.ClassifyText(It.IsAny<string>()))
            .Returns(fakeResponse);
        
        var service = new ModerationIA(mockClient.Object);
        
        var result = service.Moderate("I want to kill them.");
        
        Assert.IsTrue(result.Flagged);
        Assert.IsTrue(result.Violence);
        Assert.IsFalse(result.SelfHarm);
        Assert.IsFalse(result.Sexual);
        Assert.IsFalse(result.Hate);

    }
    
}