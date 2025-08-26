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
        int retries = 3;
        int delay = 2000; // 2 segundos

        for (int i = 0; i < retries; i++)
        {
            try
            {
                var sdkResult = _client.ClassifyText(input).Value;
                return new ModerationResultDto(IllicitOrViolence: sdkResult.Violence.Flagged);
            }
            catch (System.ClientModel.ClientResultException ex) when (ex.Status == 429)
            {
                if (i == retries - 1)
                    throw; // última tentativa, propaga o erro

                Thread.Sleep(delay);
                delay *= 2; // backoff exponencial
            }
        }

        // nunca chega aqui
        return new ModerationResultDto(IllicitOrViolence: false);
    }
}
