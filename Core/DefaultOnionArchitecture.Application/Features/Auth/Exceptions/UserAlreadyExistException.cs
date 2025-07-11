﻿using DefaultOnionArchitecture.Application.Bases;

namespace DefaultOnionArchitecture.Application.Features.Auth.Exceptions;

public class UserAlreadyExistException : BaseException
{
    public UserAlreadyExistException() : base("Böyle bir kullanıcı zaten var.") { }
    
}


