namespace Moderation.Services;

public class Text
{
    
    public TextAnalysiResult Analyze(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return new TextAnalysiResult
            {
                IllicitOccurrences = 0,
                ViolenceOccurrences = 0
            };
        }
        
        List<string> wordsToFindIlicit = new List<string>
        {
            "bater", "chutar", "morder", "arranhar", "furar", "ameaçar", "ameaçou", "sangrar"
        };
        
        List<string> wordToFindViolence = new List<string>
        {
            "matar","mato","enterrar","atirar","mordeu"
        };
        
        int illicitCount = 0;
        int violenceCount = 0;
        
        foreach (var word in wordsToFindIlicit)
        {
            if (input.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                illicitCount++;
            }
        }

        foreach (var word in wordToFindViolence)
        {
            if (input.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                violenceCount++;
            }
        }

        return new TextAnalysiResult()
        {
            IllicitOccurrences = illicitCount,
            ViolenceOccurrences = violenceCount
        };

    }
}