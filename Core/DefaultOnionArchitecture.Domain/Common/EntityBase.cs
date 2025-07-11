using System.ComponentModel.DataAnnotations;

namespace DefaultOnionArchitecture.Domain.Common;

public class EntityBase<TId> : IEntityBase
{
    [Key]
    public TId? Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
