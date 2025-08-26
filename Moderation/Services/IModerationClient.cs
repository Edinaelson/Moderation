using Moderation.DTO;

namespace Moderation.Interfaces;

public interface IModerationClient
{
    Task<bool> IsIllicitOrViolentAsync(string text);
}