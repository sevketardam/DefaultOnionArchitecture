using DefaultOnionArchitecture.Domain.Common;

namespace DefaultOnionArchitecture.Domain.Entities;

public class MetaTag : SimpleEntityBase<int>
{
    public MetaTag()
    {
        
    }

    public MetaTag(string? pageKeys,string attributeName,string attributeValue,string content,int languageId,bool isActive)
    {
        PageKeys = pageKeys;
        AttributeName = attributeName;
        AttributeValue = attributeValue;
        Content = content;
        LanguageId = languageId;
        IsActive = isActive;
    }

    public string? PageKeys { get; set; } 
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string Content { get; set; }
    public int LanguageId { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.Now;


    public Language Language { get; set; }
}
