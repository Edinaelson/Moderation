namespace Moderation.Services;

public class Text
{
    public int Input(string input)
    {
        List<string> wordsToFind = new List<string>{"bater", "chutar", "atirar", "morder", "arranhar", "furar", "ameçar", "enterrar"};
        int occurrence = 0;
        foreach (var word in wordsToFind)
        {
            if (input.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                occurrence++;
            }
        }
        
        return occurrence;
    }
}