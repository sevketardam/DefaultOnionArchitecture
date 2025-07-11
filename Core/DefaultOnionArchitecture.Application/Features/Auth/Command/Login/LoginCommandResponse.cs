namespace DefaultOnionArchitecture.Application.Features.Auth.Command.Login;

public class LoginCommandResponse
{
    public int Result { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
