using DefaultOnionArchitecture.Application.Bases;
using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Exceptions;
using DefaultOnionArchitecture.Domain.Entities;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Rules; 

public class MetaTagRules : BaseRules
{
    public Task MetaTagCannotBeNullException(MetaTag? metaTag)
    {
        if (metaTag is null) throw new MetaTagCannotBeNullException();

        return Task.CompletedTask;
    }
}
