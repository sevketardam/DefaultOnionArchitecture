﻿using DefaultOnionArchitecture.Application.Bases;

namespace DefaultOnionArchitecture.Application.Features.Auth.Exceptions;

public class RefreshTokenShouldNotBeExpiredException : BaseException
{
    public RefreshTokenShouldNotBeExpiredException() : base("Oturum süresi sona ermiştir. Lütfen tekrar giriş yapın.") { }
}


