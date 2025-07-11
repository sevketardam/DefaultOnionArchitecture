using DefaultOnionArchitecture.Application.Interface.Repositories;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Persistence.Context;
using DefaultOnionArchitecture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DefaultOnionArchitecture.Persistence.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext dbContext;
    public UnitOfWork(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async ValueTask DisposeAsync()
        => await dbContext.DisposeAsync();

    public int Save()
        => dbContext.SaveChanges();

    public async Task<int> SaveAsync()
        => await dbContext.SaveChangesAsync();

    IReadRepository<T> IUnitOfWork.GetReadRepository<T>()
        => new ReadRepository<T>(dbContext);

    IWriteRepository<T> IUnitOfWork.GetWriteRepository<T>()
        => new WriteRepository<T>(dbContext);
    public async Task<IDbContextTransaction> BeginTransactionAsync()
       => await dbContext.Database.BeginTransactionAsync();

}
