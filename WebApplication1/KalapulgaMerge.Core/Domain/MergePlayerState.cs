namespace KalapulgaMerge.Core.Domain;

public class MergePlayerState
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int Coins { get; set; }
    public string UnlockedItemsJson { get; set; } = string.Empty;
    public string ActiveEquipmentJson { get; set; } = string.Empty;
    public string Theme { get; set; } = "original";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
