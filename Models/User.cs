namespace BloxBin.Models;
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public List<Bin>? Bins { get; set; }
}