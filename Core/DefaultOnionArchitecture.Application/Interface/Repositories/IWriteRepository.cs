using DefaultOnionArchitecture.Domain.Common;
using DefaultOnionArchitecture.Domain.Entities;

namespace DefaultOnionArchitecture.Application.Interface.Repositories;

public interface IWriteRepository<T> where T : class, IEntityBase, new()
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IList<T> entities);
    Task<T> UpdateAsync(T entity);
    Task HardDeleteAsync(T entity);
    Task HardDeleteRangeAsync(IList<T> entity);
    Task SoftDeleteAsync(T entity);
    Task SoftDeleteRangeAsync(IList<T> entity);
}
