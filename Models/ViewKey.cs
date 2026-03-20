namespace BloxBin.Models;
public class ViewKey
{
    public Guid Id { get; set; }
    public string KeyHash { get; set; } = string.Empty;

    public Guid BinId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Bin? Bin { get; set; }

}