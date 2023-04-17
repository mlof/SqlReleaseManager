using MediatR;
using Microsoft.EntityFrameworkCore;
using SqlReleaseManager.Core.Abstractions;
using SqlReleaseManager.Core.Persistence;

namespace SqlReleaseManager.Web.Controllers.Mediatr;

public class DeployDatabaseHandler : IRequestHandler<DeployDatabase>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDacpacRepository _dacpacRepository;
    private readonly IDeploymentService _deploymentService;

    public DeployDatabaseHandler(ApplicationDbContext dbContext, IDacpacRepository dacpacRepository, IDeploymentService deploymentService)
    {
        _dbContext = dbContext;
        _dacpacRepository = dacpacRepository;
        _deploymentService = deploymentService;
    }

    public async Task Handle(DeployDatabase request, CancellationToken cancellationToken)
    {
        // get configuration for database

        var databaseInstance = await _dbContext.DatabaseInstances.Include(instance => instance.DeploymentConfiguration).SingleAsync(
            x => x.SqlServerInstanceId == request.ServerId && x.DatabaseName == request.DatabaseName,
            cancellationToken: cancellationToken);

        var packageName = request.Package ?? databaseInstance.DatabasePackage.FileName;
        var package = _dacpacRepository.Retrieve(packageName);


        var deploymentConfiguration = databaseInstance.DeploymentConfiguration;




    }
}