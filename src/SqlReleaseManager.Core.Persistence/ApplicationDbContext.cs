using Mapster;
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

[AdaptTo("SqlServerInstanceDto")]
public class SqlServerInstance
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string ConnectionString { get; set; }


    public List<DatabaseInstance> Databases { get; set; } = new List<DatabaseInstance>();
}

public class DatabaseInstance
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DatabaseName { get; set; }

    public int SqlServerInstanceId { get; set; }


    public int DeploymentConfigurationId { get; set; }
    public DeploymentConfiguration? DeploymentConfiguration { get; set; }


    public SqlServerInstance SqlServerInstance { get; set; }

    public List<Deployment> Deployments { get; set; } = new List<Deployment>();
}

public class DeploymentConfiguration
{
    public int Id { get; set; }

    public string Name { get; set; }
    public bool IgnoreColumnOrder { get; set; }
}

public class Deployment
{
    public int Id { get; set; }


    public string FileName { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DeploymentType DeploymentType { get; set; }

    
    public int DatabaseInstanceId { get; set; }

    public DatabaseInstance DatabaseInstance { get; set; }


}

public enum DeploymentType
{
    Report, Deploy
}