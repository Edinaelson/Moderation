
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

    [Test]
    public void CountOffensiveWords_ShouldReturnOneForSingleOfenssiveWord()
    {
        string input = "o menino mordeu a menina sem querer";
        int result = _textService.Input(input);
        
        Assert.AreEqual(1, result);
    }

    [Test]
    public void CountOffensiveWords_ShouldReturnTwoForTwoOfenssiveWords()
    {
        string input = "o menino queria bater na porta e chutar";
        int result = _textService.Input(input);
        
        Assert.AreEqual(2, result);
    }
    
    [Test]
    public void CountOffensiveWords_ShouldReturnTreForTreOfenssiveWords()
    {
        string input = "o menino queria bater, ameaçou ainda a pessoa e quis chutar na porta";
        int result = _textService.Input(input);
        
        Assert.AreEqual(3, result);
    }
    
    
}