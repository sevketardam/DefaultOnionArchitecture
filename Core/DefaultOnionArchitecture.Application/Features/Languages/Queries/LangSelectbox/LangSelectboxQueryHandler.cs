using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DefaultOnionArchitecture.Application.Features.Languages.Queries.LangSelectbox;

public class LangSelectboxQueryHandler : IRequestHandler<LangSelectboxQueryRequest, IList<SelectListItem>>
{
    private readonly IUnitOfWork unitOfWork;

    public LangSelectboxQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IList<SelectListItem>> Handle(LangSelectboxQueryRequest request, CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.GetReadRepository<Language>().GetAllAsync(a => !a.IsDeleted);

        return categories.Select(c => new SelectListItem
        {
            Text = c.Lang,
            Value = c.Id.ToString(),
            Selected = c.LangShort == "tr"
        }).ToList();
    }
}
