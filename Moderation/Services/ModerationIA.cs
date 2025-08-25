using Moderation.DTO;
using Moderation.Interfaces;

namespace Moderation.Services;

public class ModerationIA
{
   
    private readonly IModerationClient _client;

    public ModerationIA(IModerationClient client)
    {
        _client = client;
    }

    public ModerationResultDto Moderate(string textToModerate)
    {
        return _client.ClassifyText(textToModerate);
    }
    
}