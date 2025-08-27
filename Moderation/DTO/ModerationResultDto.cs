namespace Moderation.DTO;

public record ModerationResultDto(
    bool IllicitOrViolence,
    string? Text = null
    );