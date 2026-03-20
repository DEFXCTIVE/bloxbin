namespace BloxBin.Models;
public class Bin
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public bool IsPrivate { get; set; }

    public Guid OwnerId { get; set; }
    public User? User { get; set; }

    
}