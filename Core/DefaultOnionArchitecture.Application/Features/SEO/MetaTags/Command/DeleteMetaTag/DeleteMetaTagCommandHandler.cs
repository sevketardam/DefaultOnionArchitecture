using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Rules;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.DeleteMetaTag;

public class DeleteMetaTagCommandHandler(IUnitOfWork unitOfWork, MetaTagRules metaTagRules) : IRequestHandler<DeleteMetaTagCommandRequest, Unit>
{
    public async Task<Unit> Handle(DeleteMetaTagCommandRequest request, CancellationToken cancellationToken)
    {
        var metaTag = await unitOfWork.GetReadRepository<MetaTag>()
            .GetAsync(a => a.Id == request.Id);
        await metaTagRules.MetaTagCannotBeNullException(metaTag);
        await unitOfWork.GetWriteRepository<MetaTag>().HardDeleteAsync(metaTag);
        await unitOfWork.SaveAsync();
        return Unit.Value;
    }
}
