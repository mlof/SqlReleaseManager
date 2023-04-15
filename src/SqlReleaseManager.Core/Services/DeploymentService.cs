using System.Text;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Deployment;
using Microsoft.SqlServer.Dac.Model;
using SqlReleaseManager.Core.Abstractions;
using SqlReleaseManager.Core.Persistence;

namespace SqlReleaseManager.Core.Services;

public class DeploymentService : IDeploymentService
{
    private readonly ISqlServerRepository _sqlServerRepository;
    private readonly IDacpacRepository _dacpacRepository;

    public DeploymentService(ISqlServerRepository sqlServerRepository, IDacpacRepository dacpacRepository)
    {
        _sqlServerRepository = sqlServerRepository;
        _dacpacRepository = dacpacRepository;
    }

    public async Task GetDeploymentReport(string serverName, string databaseName, string dacpacName)
    {
        var server = await _sqlServerRepository.GetByName(serverName);


        var deploymentConfiguration = new DeploymentConfiguration(
        {
        };

        var dacpac = _dacpacRepository.RetrieveStream(dacpacName);


        var loadOptions = new ModelLoadOptions();

        var extractOptions = new ModelExtractOptions();

        var dacpacModel = TSqlModel.LoadFromDacpac(dacpac, loadOptions);

        var serverModel = TSqlModel.LoadFromDatabase(server.ConnectionString, extractOptions);
    }

    public Task GetDeploymentReport(TSqlModel source, TSqlModel target)
    {
        var s = Compare(source, target, Table.TypeClass);


        return Task.CompletedTask;
    }

    private IEnumerable<DeploymentDifferences> Compare(TSqlModel source, TSqlModel target,
        ModelTypeClass modelTypeClass)
    {
        var sourceObjects = source.GetObjects(DacQueryScopes.UserDefined, modelTypeClass).ToList();

        var targetObjects = target.GetObjects(DacQueryScopes.UserDefined, modelTypeClass).ToList();

        var differences = new List<DeploymentDifferences>();

        var distinct = sourceObjects.Select(o => o.Name.ToString()).Concat(targetObjects.Select(o => o.Name.ToString()))
            .Distinct();

        foreach (var name in distinct)
        {
            var diff = new DeploymentDifferences
            {
                Name = name
            };

            var sourceObject =
                sourceObjects.FirstOrDefault(o => o.Name.ToString() == name);

            var targetObject =
                targetObjects.FirstOrDefault(o => o.Name.ToString() == name);

            if (sourceObject != null)
            {
                diff.Source = sourceObject.GetScript();
            }

            if (targetObject != null)
            {
                diff.Target = targetObject.GetScript();
            }

     

            differences.Add(diff);
        }


        return differences;
    }



    private DacDeployOptions ToDacDeployOptions(DeploymentConfiguration configuration)
    {
        var options = new DacDeployOptions()
        {
            IgnoreColumnOrder = configuration.IgnoreColumnOrder,
        };
        return options;
    }
}

public record DeploymentReport
{
    IEnumerable<DeploymentDifferences> Differences { get; init; }
}

public record DeploymentDifferences
{
    public string Name { get; set; }
    public string? Source { get; set; }
    public string? Target { get; set; }
}