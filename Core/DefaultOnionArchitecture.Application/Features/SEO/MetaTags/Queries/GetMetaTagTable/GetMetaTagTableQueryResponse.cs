namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTagTable;

public class GetMetaTagTableQueryResponse
{
    public int Id { get; set; }
    public string PageKeys { get; set; }
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string Content { get; set; }
    public string Language { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedDate { get; set; }
}
