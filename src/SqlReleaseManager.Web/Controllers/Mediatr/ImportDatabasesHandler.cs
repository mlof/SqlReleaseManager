using MediatR;
using SqlReleaseManager.Core.Abstractions;

namespace SqlReleaseManager.Web.Controllers.Mediatr;

public class ImportDatabasesHandler : IRequestHandler<ImportDatabases>
{
    private readonly ISqlServerService _sqlServerService;

    public ImportDatabasesHandler(
        ISqlServerService sqlServerService
    )
    {
        _sqlServerService = sqlServerService;
    }

    public async Task Handle(ImportDatabases request, CancellationToken cancellationToken)
    {
        await _sqlServerService.UpdateDatabases(request.ServerId);
    }
}