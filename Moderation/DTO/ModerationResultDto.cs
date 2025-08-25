using OpenAI.Moderations;

namespace Moderation.DTO;

public record ModerationResultDto(
    bool Flagged,
    bool Violence,
    bool SelfHarm,
    bool Sexual,
    bool Hate
    );