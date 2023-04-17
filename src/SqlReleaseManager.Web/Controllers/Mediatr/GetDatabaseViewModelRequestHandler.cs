using MediatR;
using Microsoft.EntityFrameworkCore;
using SqlReleaseManager.Core.Persistence;
using SqlReleaseManager.Web.Models;

namespace SqlReleaseManager.Web.Controllers.Mediatr;

public class GetDatabaseViewModelRequestHandler : IRequestHandler<GetDatabaseViewModel, DatabaseViewModel>
{
    private readonly ApplicationDbContext _dbContext;

    public GetDatabaseViewModelRequestHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DatabaseViewModel> Handle(GetDatabaseViewModel request, CancellationToken cancellationToken)
    {
        return await _dbContext.DatabaseInstances.Select(databaseInstance => new DatabaseViewModel()
        {
            ServerId = databaseInstance.SqlServerInstanceId,
            Name = databaseInstance.Name,
        }).SingleAsync(model => model.Name == request.DatabaseName && model.ServerId == request.Id, cancellationToken);
    }
}