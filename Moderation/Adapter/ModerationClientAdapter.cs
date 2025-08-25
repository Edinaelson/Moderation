using Moderation.DTO;
using Moderation.Interfaces;
using OpenAI.Moderations;

namespace Moderation.Adapter;

public class ModerationClientAdapter : IModerationClient
{
    private readonly ModerationClient _client;

    public ModerationClientAdapter(ModerationClient client)
    {
        _client = client;
    }

    public ModerationResultDto ClassifyText(string input)
    {
        
        var sdkResult = _client.ClassifyText(input).Value;

        return new ModerationResultDto(
            Flagged: sdkResult.Flagged,
            Violence: sdkResult.Violence.Flagged,
            SelfHarm: sdkResult.SelfHarm.Flagged,
            Sexual: sdkResult.Sexual.Flagged,
            Hate: sdkResult.Hate.Flagged
        ); 
    }
}