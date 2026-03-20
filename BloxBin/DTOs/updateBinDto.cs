namespace BloxBin.DTOs;
public class UpdateBinDto
{
    public string? Name { get; set; }
    public string? Content { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool? IsPrivate { get; set; }
}