namespace KalapulgaMerge.Core.Domain;

public class MergeScore
{
    public int Id { get; set; }
    public int UserAccountId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Mode { get; set; } = "normal";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
