using DefaultOnionArchitecture.Application.Interface.SEO;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.Robots.Command.UpdateRobotsTxt;

public class UpdateRobotsTxtCommandHandler(IRobotsTxtFileService robotsService) : IRequestHandler<UpdateRobotsTxtCommandRequest,Unit>
{
    public async Task<Unit> Handle(UpdateRobotsTxtCommandRequest request, CancellationToken cancellationToken)
    {
        await robotsService.UpdateAsync(request.Content);
        return Unit.Value;
    }
}
