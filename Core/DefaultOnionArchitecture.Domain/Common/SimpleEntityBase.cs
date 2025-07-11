using System.ComponentModel.DataAnnotations;

namespace DefaultOnionArchitecture.Domain.Common;

public class SimpleEntityBase<TId> : IEntityBase
{
    [Key]
    public TId? Id { get; set; }
}
