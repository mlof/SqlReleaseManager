namespace SqlReleaseManager.Core.Services;

public record DeploymentReport
{
    IEnumerable<DeploymentDifferences> Differences { get; init; }
}