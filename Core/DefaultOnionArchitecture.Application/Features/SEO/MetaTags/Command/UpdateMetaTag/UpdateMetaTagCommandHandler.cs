using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Rules;
using DefaultOnionArchitecture.Application.Interface.AutoMapper;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.UpdateMetaTag;

public class UpdateMetaTagCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, MetaTagRules metaTagRules) : IRequestHandler<UpdateMetaTagCommandRequest, Unit>
{
    public async Task<Unit> Handle(UpdateMetaTagCommandRequest request, CancellationToken cancellationToken)
    {
        var metaTag = await unitOfWork.GetReadRepository<MetaTag>().GetAsync(a => a.Id == request.Id);
        await metaTagRules.MetaTagCannotBeNullException(metaTag);

        var map = mapper.Map<MetaTag, UpdateMetaTagCommandRequest>(request);

        await unitOfWork.GetWriteRepository<MetaTag>().UpdateAsync(map);
        await unitOfWork.SaveAsync();
        return Unit.Value;
    }
}
