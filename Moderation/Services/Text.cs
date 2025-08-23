namespace Moderation.Services;

public class Text
{
    public int Input(string input)
    {
        List<string> wordsToFindIlicit = new List<string>
        {
            "bater", "chutar", "morder", "arranhar", "furar", "ameçar", "ameaçou", "sangrar"
        };

        List<string> wordToFindViolence = new List<string>
        {
            "matar","mato","enterrar","atirar","mordeu"
        };
        
        //TODO implementar categorias (ilicito, licito, outro);
        
        int occurrence = 0;
        int occurrenceViolence = 0;
        
        foreach (var word in wordsToFindIlicit)
        {
            if (input.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                occurrence++;
            }
        }

        foreach (var word in wordToFindViolence)
        {
            if (input.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                occurrenceViolence++;
            }
        }
        
        return occurrence;
    }
}