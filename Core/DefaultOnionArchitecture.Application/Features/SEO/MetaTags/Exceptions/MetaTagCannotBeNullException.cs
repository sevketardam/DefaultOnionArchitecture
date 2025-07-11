using DefaultOnionArchitecture.Application.Bases;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Exceptions;

public class MetaTagCannotBeNullException : BaseException
{
    public MetaTagCannotBeNullException(string msg) : base(msg) { }
    public MetaTagCannotBeNullException() : base("MetaTag boş olamaz") { }
}
