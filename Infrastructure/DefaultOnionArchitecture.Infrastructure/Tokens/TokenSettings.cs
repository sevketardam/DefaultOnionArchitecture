namespace DefaultOnionArchitecture.Infrastructure.Tokens;

public class TokenSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public int TokenValidityInMunitues { get; set; }
}
