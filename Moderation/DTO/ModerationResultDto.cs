using OpenAI.Moderations;

namespace Moderation.DTO;

public record ModerationResultDto(bool IllicitOrViolence);