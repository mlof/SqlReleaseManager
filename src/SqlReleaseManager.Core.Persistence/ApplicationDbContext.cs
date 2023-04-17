using Microsoft.EntityFrameworkCore;

namespace SqlReleaseManager.Core.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<SqlServerInstance> SqlServerInstances { get; set; } = null!;
    public DbSet<DatabaseInstance> DatabaseInstances { get; set; } = null!;
    public DbSet<DeploymentConfiguration> DeploymentConfigurations { get; set; } = null!;
    public DbSet<Deployment> Deployments { get; set; } = null!;


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}