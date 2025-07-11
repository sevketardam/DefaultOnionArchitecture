namespace DefaultOnionArchitecture.Application.DTOs;

public class SitemapItemDto
{
    public string Loc { get; set; } = string.Empty;
    public DateTime LastMod { get; set; }
    public string ChangeFreq { get; set; } = "weekly"; // daily, monthly, etc.
    public double Priority { get; set; } = 0.5;
}
