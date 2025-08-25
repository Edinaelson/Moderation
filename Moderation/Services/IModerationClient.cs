using Moderation.DTO;

namespace Moderation.Interfaces;

public interface IModerationClient
{
        ModerationResultDto ClassifyText(string input);
}