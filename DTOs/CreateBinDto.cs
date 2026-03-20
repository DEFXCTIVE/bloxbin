namespace BloxBin.DTOs;
public class CreateBinDto
{
    public string? Name { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
    public bool IsPrivate { get; set; }
}