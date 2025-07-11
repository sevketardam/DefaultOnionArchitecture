namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTag;

public class GetMetaTagQueryResponse
{
    public int Id { get; set; }
    public string PageKeys { get; set; }
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string Content { get; set; }
    public int LanguageId { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public string CreatedDate { get; set; }
}
