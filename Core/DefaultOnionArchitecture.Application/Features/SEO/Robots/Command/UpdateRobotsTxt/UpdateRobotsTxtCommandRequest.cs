using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.Robots.Command.UpdateRobotsTxt;

public class UpdateRobotsTxtCommandRequest : IRequest<Unit>
{
    public string Content { get; set; } = string.Empty;
}
