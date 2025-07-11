using DefaultOnionArchitecture.Domain.Common;

namespace DefaultOnionArchitecture.Domain.Entities;

public class Language : SimpleEntityBase<int>
{
    public string Lang { get; set; }
    public string LangShort { get; set; }
    public string LangIcon { get; set; }
    public bool IsActive { get; set; }


    public bool IsDeleted { get; set; }

    public ICollection<MetaTag> MetaTags { get; set; }
}
