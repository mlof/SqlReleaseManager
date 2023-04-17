namespace SqlReleaseManager.Core.Persistence;

public class DeploymentConfiguration
{
    public int Id { get; set; }

    public string Name { get; set; }
    public bool IgnoreColumnOrder { get; set; }
}