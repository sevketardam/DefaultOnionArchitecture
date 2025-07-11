using MediatR;

namespace DefaultOnionArchitecture.Application.Features.Languages.Queries.GetAllLang;

public class GetAllLangQueryRequest : IRequest<IList<GetAllLangQueryResponse>>
{
    public bool GetActive { get; set; } = false;
}
