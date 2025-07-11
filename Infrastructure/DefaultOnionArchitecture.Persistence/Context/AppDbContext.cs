using System.Reflection;
using DefaultOnionArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DefaultOnionArchitecture.Persistence.Context;

public class AppDbContext : IdentityDbContext<User,Role,Guid>
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<Language> Language { get; set; }
    public DbSet<MetaTag> MetaTag { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}
