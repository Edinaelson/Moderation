
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
    public void Analyze_ShouldReturnCorrectCountsForMixedInput()
    {
        string input = "Ele quis ameaçar a garota e quis bater na porta. Depois quis atirar.";
        TextAnalysiResult result = _textService.Analyze(input);
        
        Assert.AreEqual(2, result.IllicitOccurrences);
        Assert.AreEqual(1, result.ViolenceOccurrences);
    }

    [Test]
    public void AnalyzeShouldReturnOneIllicitWord()
    {
        string input = "o menino quis morder a menina";
        TextAnalysiResult result = _textService.Analyze(input);
        
        Assert.AreEqual(1, result.IllicitOccurrences);
    }

    [Test]
    public void AnalyzeShouldReturnTwoWordsViolences()
    {
        string input = "eu vou matar voce e te enterrar";
        TextAnalysiResult result = _textService.Analyze(input);
        
        Assert.AreEqual(2, result.ViolenceOccurrences);
    }
    
    [Test]
    public void AnalyzeShouldReturnZeroOccurrences()
    {
        string input = " ele ama muito ela quer casar algum dia";
        TextAnalysiResult result = _textService.Analyze(input);
        
        Assert.AreEqual(0, result.IllicitOccurrences);
        Assert.AreEqual(0, result.ViolenceOccurrences);
    }
    
}