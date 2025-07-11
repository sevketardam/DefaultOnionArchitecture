using DefaultOnionArchitecture.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace DefaultOnionArchitecture.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedDate { get; set; }
}
