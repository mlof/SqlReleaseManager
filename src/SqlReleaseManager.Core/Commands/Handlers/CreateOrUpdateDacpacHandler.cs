using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SqlReleaseManager.Core.Commands.Handlers
{
    public class CreateOrUpdateDacpacHandler : IRequestHandler<CreateOrUpdateDacpac>
    {
        public Task Handle(CreateOrUpdateDacpac request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
