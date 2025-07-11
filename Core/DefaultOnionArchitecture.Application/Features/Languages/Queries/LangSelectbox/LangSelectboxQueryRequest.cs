using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DefaultOnionArchitecture.Application.Features.Languages.Queries.LangSelectbox;

public class LangSelectboxQueryRequest : IRequest<IList<SelectListItem>>
{
}
