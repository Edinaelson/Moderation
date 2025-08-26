using Moderation.Interfaces;

namespace Moderation.Services;

public class ModerationIA
{
   
    private readonly IModerationClient _client;

    public ModerationIA(IModerationClient client)
    {
        _client = client;
    }

    public Task<bool> Moderate(string textToModerate)
    {
        return  _client.IsIllicitOrViolentAsync(textToModerate);
    }
    
}