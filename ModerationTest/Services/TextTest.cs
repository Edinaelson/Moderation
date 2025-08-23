
namespace Moderation.Services;

[TestFixture]
public class TextTest
{
    private Text _textService;

    [SetUp]
    public void Setup()
    {
        _textService = new();
    }

    [Test]
    public void CountOffensiveWords_ShouldReturnZeroForCleanInput()
    {
        string input = "A menina estava brincando com seu cachorrinho";
        int result = _textService.Input(input);
        
        Assert.AreEqual(0, result);
    }
    
    
}