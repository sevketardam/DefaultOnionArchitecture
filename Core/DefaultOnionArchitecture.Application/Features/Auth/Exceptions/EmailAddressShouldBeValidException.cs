using DefaultOnionArchitecture.Application.Bases;

namespace DefaultOnionArchitecture.Application.Features.Auth.Exceptions;

public class EmailAddressShouldBeValidException : BaseException
{
    public EmailAddressShouldBeValidException() : base("Böyle bir e-posta adresi bulunmamaktadır.") { }

}





