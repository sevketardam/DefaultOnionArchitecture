using DefaultOnionArchitecture.Application.Interface.Repositories;
using DefaultOnionArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace DefaultOnionArchitecture.Persistence.Repositories;

public class WriteRepository<T> : IWriteRepository<T> where T : class, IEntityBase, new()
{
    private readonly DbContext dbContext;
    public WriteRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    private DbSet<T> Table { get => dbContext.Set<T>(); }

    public async Task AddAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public async Task AddRangeAsync(IList<T> entities)
    {
        await Table.AddRangeAsync(entities);
    }
    public async Task<T> UpdateAsync(T entity)
    {
        await Task.Run(() => Table.Update(entity));
        return entity;
    }

    public async Task HardDeleteAsync(T entity)
    {
        await Task.Run(() => Table.Remove(entity));
    }

    public async Task SoftDeleteAsync(T entity)
    {
        entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
        await UpdateAsync(entity);
    }


    public async Task SoftDeleteRangeAsync(IList<T> entities)
    {
        await Task.Run(() =>
        {
            foreach (var entity in entities)
            {
                var property = typeof(T).GetProperty("IsDeleted");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(entity, true);
                }
            }

            Table.UpdateRange(entities);
        });
    }

    public async Task HardDeleteRangeAsync(IList<T> entity)
    {
        await Task.Run(() => Table.RemoveRange(entity));
    }
}
