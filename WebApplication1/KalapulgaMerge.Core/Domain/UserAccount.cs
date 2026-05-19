namespace KalapulgaMerge.Core.Domain;

public class UserAccount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ProfilePicPath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
