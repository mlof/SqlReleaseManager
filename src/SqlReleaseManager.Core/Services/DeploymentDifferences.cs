namespace SqlReleaseManager.Core.Services;

public record DeploymentDifferences
{
    public string Name { get; set; }
    public string? Source { get; set; }
    public string? Target { get; set; }
}