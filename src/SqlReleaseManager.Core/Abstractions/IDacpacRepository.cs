using Microsoft.SqlServer.Dac;
using SqlReleaseManager.Core.Commands;
using SqlReleaseManager.Core.Models;

namespace SqlReleaseManager.Core.Abstractions;

public interface IDacpacRepository : IDisposable
{
    Task Create(CreateOrUpdateDacpac dacpac);
    Task<IEnumerable<DacpacDto>> List();
    Task Delete(string name);
    DacPackage Retrieve(string name);
    Task Update(string name, CreateOrUpdateDacpac dacpac);
}