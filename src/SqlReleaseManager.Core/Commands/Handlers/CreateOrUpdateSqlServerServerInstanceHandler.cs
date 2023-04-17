using MediatR;

namespace SqlReleaseManager.Core.Commands.Handlers;

public class CreateOrUpdateSqlServerServerInstanceHandler : IRequestHandler<CreateOrUpdateSqlServerInstance>
{
    public Task Handle(CreateOrUpdateSqlServerInstance request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}