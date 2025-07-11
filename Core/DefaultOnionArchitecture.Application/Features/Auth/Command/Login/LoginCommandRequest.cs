using System.ComponentModel;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.Auth.Command.Login;

public class LoginCommandRequest : IRequest<object>
{
    [DefaultValue("asd@asdasd.com")]
    public string Email { get; set; }

    [DefaultValue("123123")]
    public string Password { get; set; }
}
