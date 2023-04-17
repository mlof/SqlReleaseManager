using MediatR;
using Microsoft.EntityFrameworkCore;
using SqlReleaseManager.Core.Persistence;
using SqlReleaseManager.Web.Models;

namespace SqlReleaseManager.Web.Controllers.Mediatr;

public class GetSqlServerViewModelRequestHandler : IRequestHandler<GetSqlServerViewModel, ServerViewModel>
{
    private readonly ApplicationDbContext _dbContext;

    public GetSqlServerViewModelRequestHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServerViewModel> Handle(GetSqlServerViewModel request, CancellationToken cancellationToken)
    {
        return await _dbContext.SqlServerInstances.Select(serverInstance => new ServerViewModel()
        {
            Id = serverInstance.Id,
            ServerName = serverInstance.Name,
            Databases = serverInstance.Databases.Select(database => new DatabaseViewModel()
            {
                Name = database.Name,
            }).ToList()
        }).SingleAsync(model => model.Id == request.Id, cancellationToken: cancellationToken);
    }
}