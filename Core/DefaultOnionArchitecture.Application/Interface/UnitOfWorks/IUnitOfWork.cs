using DefaultOnionArchitecture.Application.Interface.Repositories;
using DefaultOnionArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace DefaultOnionArchitecture.Application.Interface.UnitOfWorks;

public interface IUnitOfWork : IAsyncDisposable
{
    IReadRepository<T> GetReadRepository<T>() where T : class, IEntityBase, new();
    IWriteRepository<T> GetWriteRepository<T>() where T : class, IEntityBase, new();
    Task<int> SaveAsync();
    int Save();
    Task<IDbContextTransaction> BeginTransactionAsync();
}
