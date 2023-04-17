namespace SqlReleaseManager.Core.Persistence;

public class DatabaseInstance
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DatabaseName { get; set; }

    public int SqlServerInstanceId { get; set; }
    
    public int DeploymentConfigurationId { get; set; }
    public DeploymentConfiguration? DeploymentConfiguration { get; set; }

    public DatabasePackage? DatabasePackage { get; set; }

    public SqlServerInstance SqlServerInstance { get; set; }

    public List<Deployment> Deployments { get; set; } = new List<Deployment>();
}