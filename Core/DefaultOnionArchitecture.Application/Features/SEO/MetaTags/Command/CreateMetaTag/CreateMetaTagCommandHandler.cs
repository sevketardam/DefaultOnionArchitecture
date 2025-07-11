using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.CreateMetaTag;

public class CreateMetaTagCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateMetaTagCommandRequest, Unit>
{
    public async Task<Unit> Handle(CreateMetaTagCommandRequest request, CancellationToken cancellationToken)
    {
        var metaTag = new MetaTag(request.PageKeys, request.AttributeName, request.AttributeValue, request.Content, request.LanguageId, request.IsActive);
        await unitOfWork.GetWriteRepository<MetaTag>().AddAsync(metaTag);
        await unitOfWork.SaveAsync();
        return Unit.Value;
    }
}
