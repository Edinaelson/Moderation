namespace Moderation.Model;

public class ModerationLog
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsIllicitOrViolent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}